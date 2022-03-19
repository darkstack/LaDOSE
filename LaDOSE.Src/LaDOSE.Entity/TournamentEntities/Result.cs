namespace LaDOSE.Entity
{
    public class Result : Context.Entity
    {

        public Result(Player player, int point, int rank, Tournament tournament)
        {
            Player = player;
            Point = point;
            Rank = rank;
            Tournament = tournament;

        }


        public Result()
        {
        }

        public int PlayerId { get; set; }
        public Player Player { get; set; }
        public int IdTournament { get; set; }
        public Tournament Tournament{ get; set; }
        public int Point { get; set; }
        public int Rank { get; set; }
    }
}