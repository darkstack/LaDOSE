using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LaDOSE.Business.Helper;
using LaDOSE.Business.Interface;
using LaDOSE.Entity;
using LaDOSE.Entity.Context;
using LaDOSE.Entity.Wordpress;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LaDOSE.Business.Service
{
    public class WordPressService : IWordPressService
    {
        private LaDOSEDbContext _context;
        private IChallongeProvider _challongeProvider;

        public WordPressService(LaDOSEDbContext context, IChallongeProvider challongeProvider)
        {
            this._context = context;
            this._challongeProvider = challongeProvider;
        }

        public List<WPEvent> GetWpEvent()
        {
            var wpEvents = _context.Set<WPEvent>().OrderByDescending(e => e.Id).Include(e => e.WPBookings)
                .ThenInclude(e => e.WPUser).Where(e => e.WPBookings.Count() != 0).Take(10).ToList();
            return wpEvents;
        }

        public bool UpdateBooking()
        {
            _context.Database.SetCommandTimeout(60);
            _context.Database.ExecuteSqlCommand("call ladoseapi.ImportEvent();");
            _context.Database.SetCommandTimeout(30);
            return true;
        }
        public List<WPUser> GetBooking(int wpEventId, Game game)
        {
            var selectedGameWpId = game.WordPressTag.Split(';');
            var currentWpEvent = _context.WPEvent.Include(e => e.WPBookings).ThenInclude(e => e.WPUser).Where(e => e.Id == wpEventId).ToList();
            List<WPBooking> bookings = currentWpEvent.SelectMany(e => e.WPBookings).ToList();
            List<WPUser> users = new List<WPUser>();
            foreach (var booking in bookings)
            {
                var reservations = WpEventDeserialize.Parse(booking.Meta);
                if (reservations != null)
                {
                    var gamesReservation = reservations.Where(e => e.Valid).Select(e => e.Name);
                    if (selectedGameWpId.Any(e => gamesReservation.Contains(e)))
                    {
                        users.Add(booking.WPUser);
                    }
                }
            }
            return users;
        }

        public List<WPUser> GetBookingOptions(int wpEventId, Game game)
        {
            var selectedGameWpId = game.WordPressTagOs.Split(';');
            var currentWpEvent = _context.WPEvent.Include(e => e.WPBookings).ThenInclude(e => e.WPUser).Where(e => e.Id == wpEventId).ToList();
            List<WPBooking> bookings = currentWpEvent.SelectMany(e => e.WPBookings).ToList();
            List<WPUser> users = new List<WPUser>();
            foreach (var booking in bookings)
            {
                var reservations = WpEventDeserialize.Parse(booking.Meta);
                if (reservations != null)
                {
                    var gamesReservation = reservations.Where(e => e.Valid).Select(e => e.Name);
                    if (selectedGameWpId.Any(e => gamesReservation.Contains(e)))
                    {
                        users.Add(booking.WPUser);
                    }
                }
            }
            return users;
        }
        public bool CreateChallonge(int gameId, int wpEventId)
        {
            var selectedGame = _context.Game.FirstOrDefault(e => e.Id == gameId);
            var selectedGameWpId = selectedGame.WordPressTag.Split(';');
            var currentWpEvent = _context.WPEvent.Include(e => e.WPBookings).ThenInclude(e => e.WPUser)
                .Where(e => e.Id == wpEventId);
            var users = currentWpEvent.SelectMany(e => e.WPBookings.Select(u => u.WPUser));

       
          
            if (selectedGame != null)
            {
              
                var currentEvent = currentWpEvent.FirstOrDefault();
                var eventDate = currentEvent.Date?.ToString("MM/dd/yy");

                var remove = currentEvent.Date?.ToString("Mdyy");
                var url = $"{remove}{selectedGame.Id}";
                var name = $"[{eventDate}]Ranking {currentEvent.Name} {selectedGame.Name}";
                var tournament = _challongeProvider.CreateTournament(name, url).Result;


                foreach (var booking in currentEvent.WPBookings)
                {
                    var reservations = WpEventDeserialize.Parse(booking.Meta);
                    if (reservations != null)
                    {
                        var gamesReservation = reservations.Where(e=>e.Valid).Select(e=>e.Name);
                       if(selectedGameWpId.Any(e => gamesReservation.Contains(e)))
                        { 
                            try
                            {
                                _challongeProvider.AddPlayer(tournament.id, booking.WPUser.Name);
                            }
                            catch
                            {
                                Console.WriteLine($"Erreur d ajout sur {booking.WPUser.Name}");
                                continue;
                            }
                        }
                        
                    }
                 
                 
                    
                }
      

                return true;
            }


            return false;
        }
    }
}