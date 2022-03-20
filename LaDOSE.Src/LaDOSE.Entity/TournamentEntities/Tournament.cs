using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace LaDOSE.Entity
{
    public class Tournament : Context.Entity
    {
        public Tournament()
        {
        }

        public Tournament(string name, int challongeId, int smashId)
        {
            Name = name;
            ChallongeId = challongeId;
            SmashId = smashId;
        }

        public int EventId { get; set; }
        public Event Event { get; set; }
        public String Name { get; set; }
        public int ChallongeId {get;set;}
        public int SmashId {get;set;}
        public int? GameId {get;set;} 
        public Game Game { get; set; }

        public bool Finish { get; set; }
        public List<Result> Results { get; set; }
        public List<Set> Sets { get; set; }






    }
}