using System;
using System.Collections.Generic;

namespace LaDOSE.Entity
{
    /// <summary>
    /// Represent a Event (Multiple tournaments)
    /// </summary>
    public class Event : Context.Entity
    {
        public Event()
        {
        }

        public Event(string name,int? smashId)
        {
            Name = name;
            SmashId = smashId;
        }

        public String Name { get; set; }
        public String SmashSlug { get; set; }
        public int? SmashId { get; set; }

        public DateTime Date { get; set; }

        public List<Tournament> Tournaments { get; set; }


    }
}