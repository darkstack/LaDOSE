using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

namespace LaDOSE.Entity
{

    
    public class Player : Context.Entity
    {
        public Player()
        {
        }


        public String Gamertag { get; set; }
        public String Name { get; set; }
        public int? ChallongeId {get;set;}
        public int? SmashId {get;set;}

        [NotMapped]
        public Boolean IsChallonge => ChallongeId.HasValue;


    }
}