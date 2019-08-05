using System;
using System.Collections.Generic;

namespace LaDOSE.Entity.Challonge
{
    public class ChallongeTournament : Context.Entity
    {
        public int ChallongeId { get; set; }
        public string Name { get; set; }
        public Game Game { get; set; }
        public List<ChallongeParticipent> Participents { get; set; }
        public string Url { get; set; }

        public DateTime Sync { get; set; }
    }
}