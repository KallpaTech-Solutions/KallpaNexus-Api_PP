namespace KallpaNexus_API.Models
{
    /// <summary>
    /// Sugerencias dejadas por visitantes sin identificación (pretotipo / feedback de producto).
    /// </summary>
    public class AnonymousRecommendation
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? PagePath { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
