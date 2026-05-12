using Microsoft.EntityFrameworkCore;
using KallpaNexus_API.Models;

namespace KallpaNexus_API.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Lead> Leads { get; set; }
        public DbSet<AnalyticsEvent> AnalyticsEvents { get; set; }
        public DbSet<AnonymousRecommendation> AnonymousRecommendations { get; set; }
        public DbSet<PrivateClubInterestResponse> PrivateClubInterestResponses { get; set; }
    }
}
