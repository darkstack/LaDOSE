using System;

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

        public String Name { get; set; }
        public int ChallongeId {get;set;}
        public int SmashId {get;set;}
        public int? GameId {get;set;} 
        public Game Game { get; set; }
        
    }
}