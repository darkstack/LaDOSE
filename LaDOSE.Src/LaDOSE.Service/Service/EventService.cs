using System;
using System.Linq;
using LaDOSE.Business.Interface;
using LaDOSE.Entity;
using LaDOSE.Entity.Context;
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

        public bool CreateChallonge(int dto)
        {
            var currentEvent = _context.Event.Include(e=>e.Games).ThenInclude(e=>e.Game).FirstOrDefault(e=>e.Id == dto);
            if (currentEvent != null)
            {
                var games = currentEvent.Games.Select(e => e.Game);
                var s = currentEvent.Date.ToString("MM/dd/yy");
                foreach (var game in games)
                {
                    var url = $"TestDev{game.Id}{game.Name}";
                    var name = $"[{s}]Ranking {currentEvent.Name}{game.Name}";
                    _challongeProvider.CreateTournament(name,url);
                }

                return true;
            }

            return false;
        }
    }
}