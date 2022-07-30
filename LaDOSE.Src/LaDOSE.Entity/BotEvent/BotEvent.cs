using System;
using System.Collections.Generic;
using System.Text;

namespace LaDOSE.Entity.BotEvent
{
    public class BotEvent : Context.Entity
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<BotEventResult> Results { get; set; }

    }
    public class BotEventResult : Context.Entity
    {
        public int BotEventId { get; set; }
        public BotEvent BotEvent { get; set; }

        public string Name { get; set; }
        public string DiscordId { get; set; }
        public bool Result { get; set; }
    }
}
