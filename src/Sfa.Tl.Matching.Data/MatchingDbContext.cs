using Microsoft.EntityFrameworkCore;
using Sfa.Tl.Matching.Domain.Models;

// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Sfa.Tl.Matching.Data
{
    public class MatchingDbContext : DbContext
    {
        public MatchingDbContext()
        {
        }

        public MatchingDbContext(DbContextOptions<MatchingDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<EmailTemplate> EmailTemplate { get; set; }
        public virtual DbSet<Employer> Employer { get; set; }
        public virtual DbSet<IndustryPlacement> IndustryPlacement { get; set; }
        public virtual DbSet<LocalAuthorityMapping> LocalAuthorityMapping { get; set; }
        public virtual DbSet<NotificationHistory> NotificationHistory { get; set; }
        public virtual DbSet<Path> Path { get; set; }
        public virtual DbSet<Provider> Provider { get; set; }
        public virtual DbSet<ProviderCourses> ProviderCourses { get; set; }
        public virtual DbSet<RoutePath> RoutePath { get; set; }
        public virtual DbSet<Route> Route { get; set; }
        public virtual DbSet<TemplatePlaceholder> TemplatePlaceholder { get; set; }
    }
}
