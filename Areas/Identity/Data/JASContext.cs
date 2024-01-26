using JAS.Areas.Identity.Data;
using JAS.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JAS.Models.Domain;

namespace JAS.Areas.Identity.Data
{
    public class JASContext : IdentityDbContext
    {
        private readonly IConfiguration Configuration;

        public JASContext(DbContextOptions<JASContext> options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;
        }

        public DbSet<Company> Company { get; set; }

        public DbSet<JobSeeker> JobSeeker { get; set; }

        public DbSet<JASUser> JASUser { get; set; }

        public DbSet<Application> Application { get; set; }

        public DbSet<City> City { get; set; }

        public DbSet<Country> Country { get; set; }

        public DbSet<CoverLetter> CoverLetter { get; set; }

        public DbSet<CV> CV { get; set; }

        public DbSet<Education> Education { get; set; }

        public DbSet<Experience> Experience { get; set; }

        public DbSet<JobCategory> JobCategory { get; set; }

        public DbSet<JobListing> JobListing { get; set; }

        public DbSet<JobSkill> JobSkill { get; set; }

        public DbSet<Proficiency> Proficiency { get; set; }

        public DbSet<RequiredSkills> RequiredSkills { get; set; }

        public DbSet<Status> Status { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = Configuration.GetConnectionString("JAS");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            modelBuilder.ApplyConfiguration(new JASUserEntityConfiguration());

            modelBuilder.Entity<IdentityUser>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<Company>()
               .HasKey(u => u.companyId);

            modelBuilder.Entity<JobSeeker>()
               .HasKey(u => u.jobSeekerId);

            modelBuilder.Entity<Application>()
               .HasKey(u => u.applicationId);

            modelBuilder.Entity<City>()
               .HasKey(u => u.cityId);

            modelBuilder.Entity<Country>()
               .HasKey(u => u.countryId);

            modelBuilder.Entity<CoverLetter>()
               .HasKey(u => u.coverLetterId);

            modelBuilder.Entity<CV>()
               .HasKey(u => u.cvId);

            modelBuilder.Entity<Education>()
               .HasKey(u => u.educationId);

            modelBuilder.Entity<Experience>()
               .HasKey(u => u.experienceId);

            modelBuilder.Entity<JobCategory>()
               .HasKey(u => u.categoryId);

            modelBuilder.Entity<JobListing>()
               .HasKey(u => u.positionId);

            modelBuilder.Entity<JobSkill>()
               .HasKey(u => u.skillId);

            modelBuilder.Entity<Proficiency>()
               .HasKey(u => u.proficiencyId);

            modelBuilder.Entity<RequiredSkills>()
               .HasKey(u => u.requiredSkillsId);

            modelBuilder.Entity<Status>()
               .HasKey(u => u.statusId);

            modelBuilder.Entity<JASUser>(entity =>
            {
                entity.Property(u => u.firstName).IsRequired();
                entity.Property(u => u.lastName).IsRequired();
            });

            modelBuilder.Entity<IdentityUser>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<JASUser>("JASUser");
        }

    }
}

public class JASUserEntityConfiguration : IEntityTypeConfiguration<JASUser>
{
    public void Configure(EntityTypeBuilder<JASUser> builder)
    {
        builder.Property(u => u.firstName).HasMaxLength(100);
        builder.Property(u => u.lastName).HasMaxLength(100);
    }
}

