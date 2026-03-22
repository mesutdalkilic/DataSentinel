# 🛡️ DataSentinel: AI-Powered KVKK Scanner

DataSentinel, PDF belgeleri içindeki Kişisel Verileri (PII) tespit eden ve **Gemini 3.1 Flash** AI modeli ile KVKK analizi yapan bir .NET Web API projesidir.

## 🚀 Özellikler
- **Regex Detection:** TCKN gibi kritik verileri otomatik tespit eder ve maskeler.
- **AI Analysis:** Gemini 3.1 Flash ile verinin risk analizini yapar (1-10 puanlama).
- **Background Processing:** Dosyalar bir kuyruk (Queue) yapısı ile arka planda işlenir.
- **Database Persistence:** Tüm tarama sonuçları SQLite üzerinde saklanır.

## 🛠️ Teknolojiler
- .NET 8.0 Web API
- Entity Framework Core (SQLite)
- Gemini AI API
- PdfPig (PDF Parsing)

## ⚙️ Kurulum
1. `appsettings.Template.json` dosyasını `appsettings.json` olarak kopyalayın.
2. Google AI Studio'dan aldığınız API Key'i yapıştırın.
3. `dotnet run` komutu ile çalıştırın.