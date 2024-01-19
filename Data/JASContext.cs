using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JAS.Data
{
    public class JASContext : IdentityDbContext
    {
        public JASContext(DbContextOptions options) : base(options)
        { 
        }

        //public DbSet<> MyProperty { get; set; }
    }
}
