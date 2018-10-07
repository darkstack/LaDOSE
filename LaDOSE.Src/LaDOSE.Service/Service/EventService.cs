using System;
using LaDOSE.Business.Interface;
using LaDOSE.Entity;
using LaDOSE.Entity.Context;

namespace LaDOSE.Business.Service
{
    public class EventService : BaseService<Event>, IEventService
    {
        public EventService(LaDOSEDbContext context) : base(context)
        {
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
    }
}