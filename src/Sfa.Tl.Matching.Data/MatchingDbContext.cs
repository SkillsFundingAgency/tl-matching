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

        public virtual DbSet<EmailHistory> EmailHistory { get; set; }
        public virtual DbSet<EmailPlaceholder> EmailPlaceholder { get; set; }
        public virtual DbSet<EmailTemplate> EmailTemplate { get; set; }
        public virtual DbSet<Employer> Employer { get; set; }
        public virtual DbSet<Opportunity> Opportunity { get; set; }
        public virtual DbSet<Path> Path { get; set; }
        public virtual DbSet<Provider> Provider { get; set; }
        public virtual DbSet<ProviderFeedbackRequestHistory> ProviderFeedbackRequestHistory { get; set; }
        public virtual DbSet<ProviderQualification> ProviderQualification { get; set; }
        public virtual DbSet<ProviderVenue> ProviderVenue { get; set; }
        public virtual DbSet<ProvisionGap> ProvisionGap { get; set; }
        public virtual DbSet<Qualification> Qualification { get; set; }
        public virtual DbSet<QualificationRoutePathMapping> QualificationRoutePathMapping { get; set; }
        public virtual DbSet<Referral> Referral { get; set; }
        public virtual DbSet<Route> Route { get; set; }
        public virtual DbSet<FunctionLog> FunctionLog { get; set; }
    }
}