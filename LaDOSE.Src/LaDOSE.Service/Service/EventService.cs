using System.Linq;
using LaDOSE.Business.Interface;
using LaDOSE.Business.Provider.SmashProvider;
using LaDOSE.Entity;
using LaDOSE.Entity.Context;
using LaDOSE.Entity.Wordpress;
using Microsoft.EntityFrameworkCore;

namespace LaDOSE.Business.Service
{
    public class EventService : BaseService<Event>, IEventService 
    {
        public EventService(LaDOSEDbContext context ) : base(context)
        {
            this._context = context;
         
        }

        public Event GetBySlug(string tournamentSlug)
        {
            return _context.Event.Include(e => e.Tournaments).FirstOrDefault(e => e.SmashSlug == tournamentSlug);
        }

        public Event GetByName(string name)
        {
            return _context.Event.Include(e=>e.Tournaments).FirstOrDefault(e => e.Name == name);
        }
    }
}