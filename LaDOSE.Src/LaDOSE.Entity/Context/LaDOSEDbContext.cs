using Microsoft.EntityFrameworkCore;

namespace LaDOSE.Entity.Context
{
    public class LaDOSEDbContext : DbContext
    {
        public DbSet<Game> Game { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Season> Season { get; set; }
        //public DbSet<SeasonGame> SeasonGame { get; set; }

        public LaDOSEDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
   

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SeasonGame>()
                .HasKey(t => new { t.SeasonId, t.GameId });



            modelBuilder.Entity<SeasonGame>()
                .HasOne(pt => pt.Season)
                .WithMany(p => p.Games)
                .HasForeignKey(pt => pt.GameId);

            modelBuilder.Entity<SeasonGame>()
                .HasOne(pt => pt.Game)
                .WithMany(p => p.Seasons)
                .HasForeignKey(pt => pt.SeasonId);

        }
    }


}