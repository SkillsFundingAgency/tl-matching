﻿using Microsoft.EntityFrameworkCore;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data
{
    // ReSharper disable UnusedMember.Global
    public class MatchingDbContext : DbContext
    {
        private readonly bool _applyQueryFilters;

        // ReSharper disable once IntroduceOptionalParameters.Global - will cause errors in tests
        public MatchingDbContext(DbContextOptions options) : this(options, true)
        {
        }

        public MatchingDbContext(DbContextOptions options, bool applyQueryFilters) : base(options)
        {
            _applyQueryFilters = applyQueryFilters;
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
        public virtual DbSet<LocalEnterprisePartnership> LocalEnterprisePartnership { get; set; }
        public virtual DbSet<PostcodeLookup> PostcodeLookup { get; set; }
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
        public virtual DbSet<MatchingServiceOpportunityReport> MatchingServiceOpportunityReport { get; set; }
        public virtual DbSet<MatchingServiceProviderEmployerReport> MatchingServiceProviderEmployerReport { get; set; }
        public virtual DbSet<MatchingServiceProviderOpportunityReport> MatchingServiceProviderOpportunityReport { get; set; }
        public virtual DbSet<OpportunityBasketItem> OpportunityBasketItem { get; set; }
        public virtual DbSet<UserCache> UserCache { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCache>()
                .Property(b => b.UrlHistory).HasColumnName("Value");

            modelBuilder.Entity<Opportunity>()
                .HasOne(o => o.Employer)
                .WithMany(e => e.Opportunity)
                .HasPrincipalKey(e => e.CrmId)
                .HasForeignKey(o => o.EmployerCrmId);

            modelBuilder.Entity<MatchingServiceOpportunityReport>().HasNoKey();
            modelBuilder.Entity<MatchingServiceProviderEmployerReport>().HasNoKey();
            modelBuilder.Entity<MatchingServiceProviderOpportunityReport>().HasNoKey();
            modelBuilder.Entity<OpportunityBasketItem>().HasNoKey();

            if (_applyQueryFilters)
            {
                modelBuilder.Entity<Route>()
                    .HasQueryFilter(post => EF.Property<bool>(post, "IsDeleted") == false);

                modelBuilder.Entity<Qualification>()
                    .HasQueryFilter(post => EF.Property<bool>(post, "IsDeleted") == false);

                modelBuilder.Entity<ProviderQualification>()
                    .HasQueryFilter(post => EF.Property<bool>(post, "IsDeleted") == false);

                modelBuilder.Entity<OpportunityItem>()
                    .HasQueryFilter(post => EF.Property<bool>(post, "IsDeleted") == false);
            }
        }
    }
}