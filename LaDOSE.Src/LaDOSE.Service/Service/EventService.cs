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

namespace LaDOSE.Business.Service
{
    public class EventService : BaseService<Event>, IEventService
    {
        private IChallongeProvider _challongeProvider;

        public EventService(LaDOSEDbContext context,IChallongeProvider challongeProvider) : base(context)
        {
            this._challongeProvider = challongeProvider;
        }

        public override Event GetById(int id)
        {
            return _context.Event.Include(e=>e.Season).Include(e=>e.Games).ThenInclude(e=>e.Game).FirstOrDefault(e=>e.Id == id);
        }

        public override Event Create(Event e)
        {
            if (e.Id != 0)
            {
                throw new Exception("Id is invalid");
            }

            var eventAdded = _context.Event.Add(e);
            _context.SaveChanges();
            return eventAdded.Entity;
        }

        public List<WPUser> GetBooking(int eventId, int wpEventId,Game game)
        {
            
            var currentEvent = _context.Event.Include(e => e.Games).ThenInclude(e => e.Game).FirstOrDefault(e => e.Id == eventId);
            var currentWpEvent = _context.WPEvent.Include(e => e.WPBookings).ThenInclude(e => e.WPUser).Where(e => e.Id == wpEventId).ToList();
            List<WPBooking> bookings = currentWpEvent.SelectMany(e => e.WPBookings).ToList();
            List<WPUser> users = new List<WPUser>();
            foreach (var booking in bookings)
            {
                PhpSerializer p = new PhpSerializer();
                var b = p.Deserialize(booking.Meta);
                Hashtable Wpbook = b as Hashtable;
                Hashtable reg = Wpbook["registration"] as Hashtable;
                Hashtable reg2 = Wpbook["booking"] as Hashtable;
                if (reg2.ContainsKey(game.Name) && ((string)reg2[game.Name]) == "1")
                {
                    booking.WPUser.WPBookings = null;
                    users.Add(booking.WPUser);
                }
                
            }

            return users;
        }
        public bool CreateChallonge(int eventId,int wpEventId)
        {
            var currentEvent = _context.Event.Include(e=>e.Games).ThenInclude(e=>e.Game).FirstOrDefault(e=>e.Id == eventId);
            var currentWpEvent = _context.WPEvent.Include(e => e.WPBookings).ThenInclude(e => e.WPUser).Where(e=>e.Id == wpEventId);
            var users = currentWpEvent.SelectMany(e => e.WPBookings.Select(u => u.WPUser));

            var userNames = users.Select(e => e.Name).Distinct().ToList();
            if (currentEvent != null)
            {
                var games = currentEvent.Games.Select(e => e.Game);
                var s = currentEvent.Date.ToString("MM/dd/yy");
                foreach (var game in games)
                {
                    var url = $"TestDev{game.Id}{game.Name}";
                    var name = $"[{s}]Ranking {currentEvent.Name}{game.Name}";
                    var tournament = _challongeProvider.CreateTournament(name,url).Result;
                    var eventGame = currentEvent.Games.FirstOrDefault(e => e.GameId == game.Id);
                    eventGame.ChallongeId = tournament.id;
                    eventGame.ChallongeUrl = tournament.url;
                    foreach (var userName in userNames)
                    {
                        try
                        {
                            _challongeProvider.AddPlayer(tournament.id, userName);
                        }
                        catch 
                        {
                            Console.WriteLine($"Erreur d ajout sur {userName}" );
                            continue;
                            
                        }
                    }
                    _context.Entry(eventGame).State = EntityState.Modified;
                   
                }

                _context.SaveChanges();
                return true;
            }

            return false;
        }
    }
}