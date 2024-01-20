using JAS.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

        public DbSet<JASUser> JASUser { get; set; }

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

            modelBuilder.Entity<JASUser>(entity =>
            {
                // Additional configurations for JASUser
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

