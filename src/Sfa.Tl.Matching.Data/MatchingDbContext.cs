using Microsoft.EntityFrameworkCore;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data
{
    public class MatchingDbContext : DbContext
    {
        public MatchingDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<Employer> Employer { get; set; }
        public virtual DbSet<Path> Path { get; set; }
        public virtual DbSet<Provider> Provider { get; set; }
        public virtual DbSet<ProviderVenue> ProviderVenue { get; set; }
        public virtual DbSet<Route> Route { get; set; }
        public virtual DbSet<RoutePathMapping> RoutePathMapping { get; set; }
    }
}