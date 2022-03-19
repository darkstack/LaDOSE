using System;
using System.Security.Cryptography.X509Certificates;

namespace LaDOSE.Entity
{

    
    public class Player : Context.Entity
    {
        public Player()
        {
        }

        public Player(string name, int? challongeId, int? smashId)
        {
            this.Name = name;
            ChallongeId = challongeId;
            SmashId = smashId;
        }

        
        public String  SmashName { get; set; }
        public String Name { get; set; }
        public int? ChallongeId {get;set;}
        public int? SmashId {get;set;}

        public Boolean IsChallonge => ChallongeId.HasValue;


    }
}