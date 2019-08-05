using System.Collections.Generic;

namespace LaDOSE.Entity.Challonge
{
    public class ChallongeParticipent : Context.Entity

    {
        public ChallongeTournament ChallongeTournament { get; set; }
        public int ChallongeTournamentId { get; set; }
        public int ChallongeId { get; set; }
        public string Name { get; set; }
        public int? Rank { get; set; }
        public bool? IsMember { get; set; }
        
    }
}