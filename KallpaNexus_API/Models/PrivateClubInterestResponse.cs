namespace KallpaNexus_API.Models
{
    /// <summary>
    /// Encuesta de interés para landing discreta (club privado, mayores de edad).
    /// </summary>
    public class PrivateClubInterestResponse
    {
        public int Id { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        /// <summary>1–5: interés en la propuesta.</summary>
        public int InterestScore { get; set; }

        /// <summary>1–5: probabilidad de visitar el local.</summary>
        public int VisitIntentScore { get; set; }

        public string? Comment { get; set; }
        public string? ContactPhone { get; set; }
        public bool ConfirmedAdult { get; set; }
    }
}
