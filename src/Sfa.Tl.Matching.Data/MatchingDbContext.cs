using Microsoft.EntityFrameworkCore;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data
{
    // ReSharper disable UnusedMember.Global
    public class MatchingDbContext : DbContext
    {
        public MatchingDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<BackgroundProcessHistory> BackgroundProcessHistory { get; set; }
        public virtual DbSet<BankHoliday> BankHoliday { get; set; }
        public virtual DbSet<EmailHistory> EmailHistory { get; set; }
        public virtual DbSet<EmailPlaceholder> EmailPlaceholder { get; set; }
        public virtual DbSet<EmailTemplate> EmailTemplate { get; set; }
        public virtual DbSet<Employer> Employer { get; set; }
        public virtual DbSet<Opportunity> Opportunity { get; set; }
        public virtual DbSet<OpportunityItem> OpportunityItem { get; set; }
        public virtual DbSet<Path> Path { get; set; }
        public virtual DbSet<Provider> Provider { get; set; }
        public virtual DbSet<LearningAimReference> LearningAimReference { get; set; }
        public virtual DbSet<ProviderReference> ProviderReference { get; set; }
        public virtual DbSet<ProviderQualification> ProviderQualification { get; set; }
        public virtual DbSet<ProviderVenue> ProviderVenue { get; set; }
        public virtual DbSet<ProvisionGap> ProvisionGap { get; set; }
        public virtual DbSet<Qualification> Qualification { get; set; }
        public virtual DbSet<QualificationRouteMapping> QualificationRouteMapping { get; set; }
        public virtual DbSet<Referral> Referral { get; set; }
        public virtual DbSet<Route> Route { get; set; }
        public virtual DbSet<FunctionLog> FunctionLog { get; set; }
        public virtual DbSet<ServiceStatusHistory> ServiceStatusHistory { get; set; }
         public virtual DbSet<UserCache> UserCache { get; set; }
       public virtual DbQuery<MatchingServiceOpportunityReport> MatchingServiceOpportunityReport { get; set; }
        public virtual DbQuery<MatchingServiceProviderOpportunityReport> MatchingServiceProviderOpportunityReport { get; set; }
        public virtual DbQuery<MatchingServiceProviderEmployerReport> MatchingServiceProviderEmployerReport { get; set; }
        public virtual DbQuery<OpportunityBasketItem> OpportunityBasketItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCache>()
                .Property(b => b.UrlHistory).HasColumnName("Value");

            modelBuilder.Entity<Opportunity>()
                .HasOne(o => o.Employer)
                .WithMany(e => e.Opportunity)
                .HasPrincipalKey(e => e.CrmId)
                .HasForeignKey(o => o.EmployerCrmId);
        }
    }
}