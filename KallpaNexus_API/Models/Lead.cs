namespace KallpaNexus_API.Models
{
    public class Lead
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string? Company { get; set; }
        public string? Sector { get; set; } // "Sport", "Stay", "Care", "Gear"

        // --- NUEVOS DATOS CLAVE ---

        // Para saber por dónde enviar la información (WhatsApp o Email)
        public string PreferredContactMethod { get; set; } = "WhatsApp";

        // Para saber qué plan les llamó más la atención (150, 250, Personalizado)
        public string? InterestedPlan { get; set; }

        // Para que dejen sus dudas o necesidades específicas
        public string? Message { get; set; }

        // Para temas legales y de protección de datos (Pretotipo profesional)
        public bool AcceptedTerms { get; set; } = false;

        // Fecha de registro automática
        public DateTime Date { get; set; } = DateTime.UtcNow;

        // Estado interno (para que tú lleves el control de a quién ya contactaste)
        public string Status { get; set; } = "Pending";
    }
}
