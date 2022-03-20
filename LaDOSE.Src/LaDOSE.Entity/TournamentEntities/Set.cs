using System.ComponentModel.DataAnnotations.Schema;

namespace LaDOSE.Entity
{
    public class Set : Context.Entity
    {
        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }


        public int Player1Id { get; set; }
        //[ForeignKey("Player1Id")]
        //public Player Player1 { get; set; }
        
        public int Player2Id { get; set; }
        //[ForeignKey("Player2Id")]
        //public Player Player2 { get; set; }

        public int Player1Score { get; set; }
        public int Player2Score { get; set; }

        public int Round { get; set; }
    }
}