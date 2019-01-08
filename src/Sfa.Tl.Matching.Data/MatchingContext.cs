using Sfa.Tl.Matching.Data.Models;

namespace Sfa.Tl.Matching.Data
{
    //EF Core Db Context
    //public class MatchingDbContext : DbContext
    //{
    //    public MatchingDbContext()
    //    {
    //    }

    //    public MatchingDbContext(DbContextOptions<MatchingContext> options)
    //        : base(options)
    //    {
    //    }

    //    public virtual DbSet<Address> Address { get; set; }
    //    public virtual DbSet<Contact> Contact { get; set; }
    //    public virtual DbSet<Course> Course { get; set; }
    //    public virtual DbSet<EmailTemplate> EmailTemplate { get; set; }
    //    public virtual DbSet<Employer> Employer { get; set; }
    //    public virtual DbSet<IndustryPlacement> IndustryPlacement { get; set; }
    //    public virtual DbSet<LocalAuthorityMapping> LocalAuthorityMapping { get; set; }
    //    public virtual DbSet<NotificationHistory> NotificationHistory { get; set; }
    //    public virtual DbSet<Provider> Provider { get; set; }
    //    public virtual DbSet<ProviderCourses> ProviderCourses { get; set; }
    //    public virtual DbSet<RoutePath> RoutePath { get; set; }
    //    public virtual DbSet<TemplatePlaceholder> TemplatePlaceholder { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    {
    //        if (!optionsBuilder.IsConfigured)
    //        {
    //            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Matching;Trusted_Connection=True;");
    //        }
    //    }

    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

    //        modelBuilder.Entity<Address>(entity =>
    //        {
    //            entity.Property(e => e.Id).ValueGeneratedNever();

    //            entity.Property(e => e.AddressLine1)
    //                .HasMaxLength(50)
    //                .IsUnicode(false);

    //            entity.Property(e => e.AddressLine2)
    //                .HasMaxLength(50)
    //                .IsUnicode(false);

    //            entity.Property(e => e.AddressLine3)
    //                .HasMaxLength(50)
    //                .IsUnicode(false);

    //            entity.Property(e => e.City)
    //                .HasMaxLength(50)
    //                .IsUnicode(false);

    //            entity.Property(e => e.Country).HasMaxLength(50);

    //            entity.Property(e => e.County).HasMaxLength(50);

    //            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

    //            entity.Property(e => e.Latitude).HasColumnType("decimal(9, 6)");

    //            entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");

    //            entity.Property(e => e.Postcode)
    //                .IsRequired()
    //                .HasMaxLength(10)
    //                .IsUnicode(false);

    //            entity.Property(e => e.Region).HasMaxLength(200);

    //            entity.HasOne(d => d.EntityRef)
    //                .WithMany(p => p.Address)
    //                .HasForeignKey(d => d.EntityRefId)
    //                .OnDelete(DeleteBehavior.ClientSetNull)
    //                .HasConstraintName("FK_Address_Employer");

    //            entity.HasOne(d => d.EntityRefNavigation)
    //                .WithMany(p => p.Address)
    //                .HasForeignKey(d => d.EntityRefId)
    //                .OnDelete(DeleteBehavior.ClientSetNull)
    //                .HasConstraintName("FK_Address_Provider");

    //            entity.HasOne(d => d.LocalAuthorityMapping)
    //                .WithMany(p => p.Address)
    //                .HasForeignKey(d => d.LocalAuthorityMappingId)
    //                .HasConstraintName("FK_Address_LocalAuthorityMapping");
    //        });

    //        modelBuilder.Entity<Contact>(entity =>
    //        {
    //            entity.Property(e => e.Id).ValueGeneratedNever();

    //            entity.Property(e => e.BusinessPhone).HasMaxLength(100);

    //            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

    //            entity.Property(e => e.Email).HasMaxLength(100);

    //            entity.Property(e => e.FirstName).HasMaxLength(100);

    //            entity.Property(e => e.HomePhone).HasMaxLength(100);

    //            entity.Property(e => e.JobTitle).HasMaxLength(100);

    //            entity.Property(e => e.LastName).HasMaxLength(100);

    //            entity.Property(e => e.MiddleName).HasMaxLength(100);

    //            entity.Property(e => e.MobilePhone).HasMaxLength(100);

    //            entity.HasOne(d => d.EntityRef)
    //                .WithMany(p => p.Contact)
    //                .HasForeignKey(d => d.EntityRefId)
    //                .OnDelete(DeleteBehavior.ClientSetNull);

    //            entity.HasOne(d => d.EntityRefNavigation)
    //                .WithMany(p => p.Contact)
    //                .HasForeignKey(d => d.EntityRefId)
    //                .OnDelete(DeleteBehavior.ClientSetNull);
    //        });

    //        modelBuilder.Entity<Course>(entity =>
    //        {
    //            entity.Property(e => e.Id).ValueGeneratedNever();

    //            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

    //            entity.Property(e => e.Keywords).HasMaxLength(50);

    //            entity.Property(e => e.QualificationTitle)
    //                .IsRequired()
    //                .HasMaxLength(100);

    //            entity.Property(e => e.Summary).HasMaxLength(50);
    //        });

    //        modelBuilder.Entity<EmailTemplate>(entity =>
    //        {
    //            entity.Property(e => e.Id).ValueGeneratedNever();

    //            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

    //            entity.Property(e => e.TemplateId)
    //                .IsRequired()
    //                .HasMaxLength(50);

    //            entity.Property(e => e.TemplateName)
    //                .IsRequired()
    //                .HasMaxLength(50);
    //        });

    //        modelBuilder.Entity<Employer>(entity =>
    //        {
    //            entity.Property(e => e.Id).ValueGeneratedNever();

    //            entity.Property(e => e.AlsoKnownAs).HasMaxLength(100);

    //            entity.Property(e => e.CompanyName)
    //                .IsRequired()
    //                .HasMaxLength(160);

    //            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

    //            entity.Property(e => e.Website).HasMaxLength(200);
    //        });

    //        modelBuilder.Entity<IndustryPlacement>(entity =>
    //        {
    //            entity.Property(e => e.Id).ValueGeneratedNever();

    //            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

    //            entity.Property(e => e.Placement).HasDefaultValueSql("((0))");

    //            entity.Property(e => e.PlacementGap).HasDefaultValueSql("((0))");

    //            entity.Property(e => e.PlacementOffered).HasDefaultValueSql("((0))");

    //            entity.Property(e => e.PlacementReferred).HasDefaultValueSql("((0))");

    //            entity.Property(e => e.Resolution).HasMaxLength(100);

    //            entity.HasOne(d => d.Address)
    //                .WithMany(p => p.IndustryPlacement)
    //                .HasForeignKey(d => d.AddressId)
    //                .HasConstraintName("FK_IndustryPlacement_Address");

    //            entity.HasOne(d => d.RoutePath)
    //                .WithMany(p => p.IndustryPlacement)
    //                .HasForeignKey(d => d.RoutePathId)
    //                .HasConstraintName("FK_IndustryPlacement_RoutePath");
    //        });

    //        modelBuilder.Entity<LocalAuthorityMapping>(entity =>
    //        {
    //            entity.Property(e => e.Id).ValueGeneratedNever();

    //            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

    //            entity.Property(e => e.LocalAuthority)
    //                .IsRequired()
    //                .HasMaxLength(50);

    //            entity.Property(e => e.LocalAuthorityCode)
    //                .IsRequired()
    //                .HasMaxLength(50);

    //            entity.Property(e => e.LocalEnterprisePartnership)
    //                .IsRequired()
    //                .HasMaxLength(50);
    //        });

    //        modelBuilder.Entity<NotificationHistory>(entity =>
    //        {
    //            entity.Property(e => e.Id).ValueGeneratedNever();

    //            entity.Property(e => e.Body)
    //                .IsRequired()
    //                .HasMaxLength(500);

    //            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

    //            entity.Property(e => e.Recipients)
    //                .IsRequired()
    //                .HasMaxLength(50);

    //            entity.Property(e => e.Sender)
    //                .IsRequired()
    //                .HasMaxLength(50);

    //            entity.Property(e => e.Subject)
    //                .IsRequired()
    //                .HasMaxLength(50);

    //            entity.HasOne(d => d.EmailTemplate)
    //                .WithMany(p => p.NotificationHistory)
    //                .HasForeignKey(d => d.EmailTemplateId)
    //                .OnDelete(DeleteBehavior.ClientSetNull)
    //                .HasConstraintName("FK_Notification_EmailTemplate");

    //            entity.HasOne(d => d.EntityRef)
    //                .WithMany(p => p.NotificationHistory)
    //                .HasForeignKey(d => d.EntityRefId)
    //                .OnDelete(DeleteBehavior.ClientSetNull)
    //                .HasConstraintName("FK_Notification_Employer");

    //            entity.HasOne(d => d.EntityRefNavigation)
    //                .WithMany(p => p.NotificationHistory)
    //                .HasForeignKey(d => d.EntityRefId)
    //                .OnDelete(DeleteBehavior.ClientSetNull)
    //                .HasConstraintName("FK_Notification_Provider");
    //        });

    //        modelBuilder.Entity<Provider>(entity =>
    //        {
    //            entity.Property(e => e.Id).ValueGeneratedNever();

    //            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

    //            entity.Property(e => e.Name)
    //                .IsRequired()
    //                .HasMaxLength(50);

    //            entity.Property(e => e.Ukprn)
    //                .IsRequired()
    //                .HasColumnName("UKPRN")
    //                .HasMaxLength(50);
    //        });

    //        modelBuilder.Entity<ProviderCourses>(entity =>
    //        {
    //            entity.Property(e => e.Id).ValueGeneratedNever();

    //            entity.HasOne(d => d.Address)
    //                .WithMany(p => p.ProviderCourses)
    //                .HasForeignKey(d => d.AddressId)
    //                .OnDelete(DeleteBehavior.ClientSetNull)
    //                .HasConstraintName("FK_ProviderCourse_Address");

    //            entity.HasOne(d => d.Course)
    //                .WithMany(p => p.ProviderCourses)
    //                .HasForeignKey(d => d.CourseId)
    //                .OnDelete(DeleteBehavior.ClientSetNull)
    //                .HasConstraintName("FK_ProviderCourse_Course");

    //            entity.HasOne(d => d.Provider)
    //                .WithMany(p => p.ProviderCourses)
    //                .HasForeignKey(d => d.ProviderId)
    //                .OnDelete(DeleteBehavior.ClientSetNull)
    //                .HasConstraintName("FK_ProviderCourse_Provider");
    //        });

    //        modelBuilder.Entity<RoutePath>(entity =>
    //        {
    //            entity.Property(e => e.Id).ValueGeneratedNever();

    //            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

    //            entity.Property(e => e.Keywords).HasMaxLength(50);

    //            entity.Property(e => e.Path)
    //                .IsRequired()
    //                .HasMaxLength(10);

    //            entity.Property(e => e.Route)
    //                .IsRequired()
    //                .HasMaxLength(50);

    //            entity.Property(e => e.Summary).HasMaxLength(50);

    //            entity.HasOne(d => d.Course)
    //                .WithMany(p => p.RoutePath)
    //                .HasForeignKey(d => d.CourseId)
    //                .OnDelete(DeleteBehavior.ClientSetNull)
    //                .HasConstraintName("FK_RoutePath_Course");
    //        });

    //        modelBuilder.Entity<TemplatePlaceholder>(entity =>
    //        {
    //            entity.Property(e => e.Id).ValueGeneratedNever();

    //            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

    //            entity.Property(e => e.Placeholder)
    //                .IsRequired()
    //                .HasMaxLength(100);

    //            entity.HasOne(d => d.EmailTemplate)
    //                .WithMany(p => p.TemplatePlaceholder)
    //                .HasForeignKey(d => d.EmailTemplateId)
    //                .OnDelete(DeleteBehavior.ClientSetNull)
    //                .HasConstraintName("FK_TemplatePlaceholder_EmailTemplate");
    //        });
    //    }
    //}
}
