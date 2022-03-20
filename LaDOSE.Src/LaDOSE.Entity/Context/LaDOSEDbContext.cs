using LaDOSE.Entity.Challonge;

using LaDOSE.Entity.Wordpress;
using Microsoft.EntityFrameworkCore;

namespace LaDOSE.Entity.Context
{
    public class LaDOSEDbContext : DbContext
    {
        public DbSet<Game> Game { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<Todo> Todo { get; set; }

        #region WordPress
        public DbSet<WPUser> WPUser { get; set; }
        public DbSet<WPEvent> WPEvent { get; set; }
        public DbSet<WPBooking> WPBooking { get; set; }


        #endregion


        #region Tournament
        public DbSet<Player> Player { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<Tournament> Tournament { get; set; }
        public DbSet<Result> Result { get; set; }
        public DbSet<Set> Set { get; set; }

        #endregion
        public DbSet<ChallongeParticipent> ChallongeParticipent { get; set; }
        public DbSet<ChallongeTournament> ChallongeTournament { get; set; }

        public LaDOSEDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
   

            base.OnModelCreating(modelBuilder);



            modelBuilder.Entity<Event>()
                .HasMany(s => s.Tournaments);


            modelBuilder.Entity<Tournament>()
                .HasOne(e => e.Game)
                .WithMany(e=>e.Tournaments)
                .HasForeignKey(pt=>pt.GameId)
                ;
            modelBuilder.Entity<Tournament>()
                .HasOne(e => e.Event)
                .WithMany(e => e.Tournaments)
                .HasForeignKey(pt => pt.EventId)
                ;

            modelBuilder.Entity<Set>()
                .HasOne(e => e.Tournament)
                .WithMany(e => e.Sets)
                .HasForeignKey(pt => pt.TournamentId)
                ;

            //modelBuilder.Entity<Set>()
            //    .HasOne(e => e.Player1)
            //    .WithMany(e => e.Sets)
            //    .HasForeignKey(pt => pt.Player1Id)
            //    ;

            //modelBuilder.Entity<Set>()
            //    .HasOne(e => e.Player2)
            //    .WithMany(e => e.Sets)
            //    .HasForeignKey(pt => pt.Player2Id)
            //    ;
            //#region SeasonGame
            //modelBuilder.Entity<SeasonGame>()
            //    .HasKey(t => new { t.SeasonId, t.GameId });

            //modelBuilder.Entity<SeasonGame>()
            //    .HasOne(pt => pt.Season)
            //    .WithMany(p => p.Games)
            //    .HasForeignKey(pt => pt.SeasonId);

            //modelBuilder.Entity<SeasonGame>()
            //    .HasOne(pt => pt.Game)
            //    .WithMany(p => p.Seasons)
            //    .HasForeignKey(pt => pt.GameId);
            //#endregion

            //#region EventGame

            //modelBuilder.Entity<T>()
            //    .HasKey(t => new { t.EventId, t.GameId });

            //modelBuilder.Entity<EventGame>()
            //    .HasOne(pt => pt.Event)
            //    .WithMany(p => p.Games)
            //    .HasForeignKey(pt => pt.EventId);

            //modelBuilder.Entity<EventGame>()
            //    .HasOne(pt => pt.Game)
            //    .WithMany(p => p.Events)
            //    .HasForeignKey(pt => pt.GameId);
            //#endregion

            #region WordPress WPBooking

            modelBuilder.Entity<WPBooking>()
                .HasKey(t => new { t.WPEventId, t.WPUserId });

            modelBuilder.Entity<WPBooking>()
                .HasOne(pt => pt.WPEvent)
                .WithMany(p => p.WPBookings)
                .HasForeignKey(pt => pt.WPEventId);

            modelBuilder.Entity<WPBooking>()
                .HasOne(pt => pt.WPUser)
                .WithMany(p => p.WPBookings)
                .HasForeignKey(pt => pt.WPUserId);
            #endregion

            #region Challonge 
            modelBuilder.Entity<ChallongeParticipent>()
                .HasOne(pt => pt.ChallongeTournament)
                .WithMany(p => p.Participents)
                .HasForeignKey(pt => pt.ChallongeTournamentId);
            #endregion
        }
    }


}