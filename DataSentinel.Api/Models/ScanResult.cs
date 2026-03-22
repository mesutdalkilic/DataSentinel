using System.ComponentModel.DataAnnotations;

namespace DataSentinel.Api.Models
{
    public class ScanResult
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FileName { get; set; } = string.Empty;
        public string Findings { get; set; } = string.Empty; // Bulgular (T.C. No maskeli hali)
        public string RiskLevel { get; set; } = "Low";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public string? AiSummary { get; set; } // AI'nın doküman hakkındaki özeti. Ayrıca AI yorumu başlangıçta boş olabilir, o yüzden '?' ekliyoruz
        public int RiskScore { get; set; }     // 1 ile 10 arası risk puanı

    }

}
