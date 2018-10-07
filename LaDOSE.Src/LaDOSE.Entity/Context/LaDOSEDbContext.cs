using Microsoft.EntityFrameworkCore;

namespace LaDOSE.Entity.Context
{
    public class LaDOSEDbContext : DbContext
    {
        public DbSet<Game> Game { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Season> Season { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<SeasonGame> SeasonGame { get; set; }
        public DbSet<EventGame> EventGame { get; set; }

        public LaDOSEDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
   

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SeasonGame>()
                .HasKey(t => new { t.SeasonId, t.GameId });
            modelBuilder.Entity<EventGame>()
                .HasKey(t => new { t.EventId, t.GameId });

            modelBuilder.Entity<Event>()
                .HasOne(s => s.Season)
                .WithMany(p => p.Event)
                .HasForeignKey(fk => fk.SeasonId);



            modelBuilder.Entity<SeasonGame>()
                .HasOne(pt => pt.Season)
                .WithMany(p => p.Games)
                .HasForeignKey(pt => pt.SeasonId);

            modelBuilder.Entity<SeasonGame>()
                .HasOne(pt => pt.Game)
                .WithMany(p => p.Seasons)
                .HasForeignKey(pt => pt.GameId);


            modelBuilder.Entity<EventGame>()
                .HasOne(pt => pt.Event)
                .WithMany(p => p.Games)
                .HasForeignKey(pt => pt.EventId);

            modelBuilder.Entity<EventGame>()
                .HasOne(pt => pt.Game)
                .WithMany(p => p.Events)
                .HasForeignKey(pt => pt.GameId);

        }
    }


}