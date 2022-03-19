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

        public EventService(LaDOSEDbContext context, IChallongeProvider challongeProvider) : base(context)
        {
            this._challongeProvider = challongeProvider;
        }

        //public override Event GetById(int id)
        //{
        //    re
        //    //return _context.Event.Include(e => e.Season).Include(e => e.Games).ThenInclude(e => e.Game)
        //    //    .FirstOrDefault(e => e.Id == id);
        //}

        //public override Event Create(Event e)
        //{
        //    if (e.Id != 0)
        //    {
        //        throw new Exception("Id is invalid");
        //    }

        //    var eventAdded = _context.Event.Add(e);
        //    _context.SaveChanges();
        //    return eventAdded.Entity;
        //}

    }
}