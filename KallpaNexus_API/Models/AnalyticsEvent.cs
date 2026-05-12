namespace KallpaNexus_API.Models
{
    public class AnalyticsEvent
    {
        public int Id { get; set; }
        public string EventType { get; set; } = string.Empty;
        public string TargetName { get; set; } = string.Empty;
        public string? Sector { get; set; }
        public string? Url { get; set; }
        public string? Referrer { get; set; }
        public string? DeviceType { get; set; }
        public string? IpAddress { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
