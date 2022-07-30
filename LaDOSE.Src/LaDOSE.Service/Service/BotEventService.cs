using System;
using System.Linq;
using LaDOSE.Business.Interface;
using LaDOSE.Entity;
using LaDOSE.Entity.BotEvent;
using LaDOSE.Entity.Context;
using Microsoft.EntityFrameworkCore;

namespace LaDOSE.Business.Service
{
    public class BotEventService : BaseService<BotEvent>, IBotEventService
    {
        public BotEventService(LaDOSEDbContext context) : base(context)
        {
            this._context = context;

        }

        public bool CreateEvent(string EventName)
        {
            this._context.BotEvent.Add(new BotEvent { Name = EventName, Date = DateTime.Now });
            return this._context.SaveChanges()!=0;
        }

        public BotEvent GetLastEvent()
        {
            return this._context.BotEvent.Include(e=>e.Results).FirstOrDefault(e => e.Date == this._context.BotEvent.Max(e => e.Date));
        }

        public bool SetResult(string discordId, string name, bool present)
        {
            if (string.IsNullOrEmpty(discordId))
                throw new Exception("DiscordId invalid.");

            BotEvent currentEvent = this.GetLastEvent();
            
            if(currentEvent == null)
            {
                throw new Exception("Oups");
            }
            BotEventResult res = currentEvent.Results.FirstOrDefault(e => e.DiscordId == discordId);
            if (res != null)
            {
                res.Result = present;
                _context.BotEventResult.Update(res);
                _context.SaveChanges();
                return true;
            }

            _context.BotEventResult.Add(new BotEventResult() { BotEventId = currentEvent.Id, DiscordId = discordId, Result = present, Name = name });
            return _context.SaveChanges()!=0;
            
        }
    }
}