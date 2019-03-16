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
using Newtonsoft.Json.Serialization;

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
        public WPEvent GetNextWpEvent()
        {
            var wpEvents = _context.Set<WPEvent>().OrderByDescending(e=>e.Date).ThenByDescending(e => e.Id)
                .Include(e => e.WPBookings).ThenInclude(e => e.WPUser).FirstOrDefault(e => Enumerable.Count<WPBooking>(e.WPBookings) != 0);
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
        public string CreateChallonge(int gameId, int wpEventId, IList<WPUser> additionalPlayers)
        {
            var selectedGame = _context.Game.FirstOrDefault(e => e.Id == gameId);
            var selectedGameWpId = selectedGame.WordPressTag.Split(';');
            var currentWpEvent = _context.WPEvent.Include(e => e.WPBookings).ThenInclude(e => e.WPUser)
                .Where(e => e.Id == wpEventId);
            var users = currentWpEvent.SelectMany(e => e.WPBookings.Select(u => u.WPUser));
            var useradded = new List<WPUser>();
       
          
            if (selectedGame != null)
            {
              
                var currentEvent = currentWpEvent.FirstOrDefault();
                var eventDate = currentEvent.Date?.ToString("dd/MM/yy");

                var remove = currentEvent.Date?.ToString("Mdyy");
                var url = $"{remove}{selectedGame.Id}";
                var selectedEvent = FormatCurrentEventName(currentEvent.Name);
                var name = $"[{eventDate}] LaDOSE.Net - {selectedEvent} - {selectedGame.Name}";
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
                                useradded.Add(booking.WPUser);
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

                if (additionalPlayers != null && additionalPlayers.Count > 0)
                {
                    foreach (var additionalPlayer in additionalPlayers)
                    {
                        if (useradded.All(e => e.Name != additionalPlayer.Name))
                        {
                            _challongeProvider.AddPlayer(tournament.id, additionalPlayer.Name);
                        }
                    }

                }


                 return tournament.url;
            }


            return "error while creating challonge";
        }

        private string FormatCurrentEventName(string currentEventName)
        {

            
            if (currentEventName.Contains("-"))
            {
                var strings = currentEventName.Split('-');
                var s = strings[strings.Length-1];
                DateTime test;
                if (DateTime.TryParse(s, out test))
                {
                    var formatCurrentEventName = currentEventName.Replace(s, "");
                    formatCurrentEventName= formatCurrentEventName.Replace(" -", "");
                    return formatCurrentEventName;
                }
                
            }

            return currentEventName;
        }
    }
}