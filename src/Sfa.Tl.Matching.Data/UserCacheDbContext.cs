using Microsoft.EntityFrameworkCore;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data
{
    public class UserCacheDbContext : MatchingDbContext
    {
        public virtual DbSet<BackLinkHistory> BackLinkHistory { get; set; }
        public virtual DbSet<UserCache> UserCache { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCache>()
                .Property(b => b.UrlHistory).HasColumnName("Value");
        }

        public UserCacheDbContext(DbContextOptions<MatchingDbContext> options) : base(options)
        {
        }
    }
}
