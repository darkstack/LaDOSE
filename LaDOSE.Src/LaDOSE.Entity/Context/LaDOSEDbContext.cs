using Microsoft.EntityFrameworkCore;

namespace LaDOSE.Entity.Context
{
    public class LaDOSEDbContext : DbContext
    {
        public DbSet<Game> Game { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Season> Season { get; set; }

        public LaDOSEDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }


}