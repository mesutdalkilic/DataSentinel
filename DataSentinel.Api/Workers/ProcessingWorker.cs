using System.Text.RegularExpressions;
using System.Net.Http.Json;
using System.Text.Json;
using DataSentinel.Api.Infrastructure;
using DataSentinel.Api.Models;
using UglyToad.PdfPig;

namespace DataSentinel.Api.Workers;

public class ProcessingWorker : BackgroundService
{
    private readonly ILogger<ProcessingWorker> _logger;
    private readonly IFileQueue _queue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly string _apiKey;

    public ProcessingWorker(ILogger<ProcessingWorker> logger, IFileQueue queue, IServiceScopeFactory scopeFactory, IConfiguration configuration)
    {
        _logger = logger;
        _queue = queue;
        _scopeFactory = scopeFactory;
        _apiKey = configuration["Gemini:ApiKey"] ?? "";
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ProcessingWorker Aktif. Kuyruk dinleniyor...");

        while (!stoppingToken.IsCancellationRequested)
        {
            var filePath = await _queue.DequeueAsync(stoppingToken);
            _logger.LogInformation("Analiz Ediliyor: {file}", Path.GetFileName(filePath));

            try
            {
                // 1. PDF'den Metin Çıkarma
                string fullText = "";
                using (var pdf = PdfDocument.Open(filePath))
                {
                    foreach (var page in pdf.GetPages()) fullText += page.Text;
                }

                // 2. REGEX ANALİZİ (T.C. No)
                var matches = Regex.Matches(fullText, @"\b[1-9][0-9]{9}[02468]\b");
                string findings = matches.Count > 0
                    ? string.Join(", ", matches.Select(m => m.Value.Substring(0, 3) + "********"))
                    : "Temiz";

                // 3. AI ANALİZİ (Google 3.1 Sürümü)
                string aiResponse = "Analiz yapılamadı.";
                using var client = new HttpClient();

                // Senin listendeki en güncel model ismini buraya yaz:
                var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-3.1-flash-lite-preview:generateContent?key={_apiKey}";

                var payload = new
                {
                    contents = new[] { new { parts = new[] { new { text = $"Aşağıdaki metni KVKK açısından analiz et, riskleri belirt ve 1-10 arası bir risk puanı ver: {fullText}" } } } }
                };

                var response = await client.PostAsJsonAsync(url, payload);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadFromJsonAsync<JsonElement>();
                    aiResponse = json.GetProperty("candidates")[0]
                                     .GetProperty("content")
                                     .GetProperty("parts")[0]
                                     .GetProperty("text").GetString() ?? "Özet boş döndü.";
                }
                else
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    _logger.LogCritical("GOOGLE HATASI! Kod: {code}, Detay: {details}", response.StatusCode, errorBody);
                    aiResponse = $"API Hatası: {response.StatusCode}";
                }

                // 4. VERİTABANINA KAYIT
                using (var scope = _scopeFactory.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<SentinelDbContext>();
                    db.ScanResults.Add(new ScanResult
                    {
                        FileName = Path.GetFileName(filePath),
                        Findings = findings,
                        AiSummary = aiResponse,
                        RiskScore = matches.Count > 0 ? 9 : 3,
                        RiskLevel = matches.Count > 0 ? "Critical" : "Low",
                        CreatedAt = DateTime.Now
                    });
                    await db.SaveChangesAsync();
                }
                _logger.LogInformation("İşlem Tamamlandı: {file}", Path.GetFileName(filePath));
            }
            catch (Exception ex)
            {
                _logger.LogError("Sistem Hatası: {m}", ex.Message);
            }
        }
    }
}