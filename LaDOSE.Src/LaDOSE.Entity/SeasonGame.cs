namespace LaDOSE.Entity
{
    public class SeasonGame
    {
        public int GameId { get; set; }
        public Game Game { get; set; }
        public int SeasonId { get; set; }
        public Season Season { get; set; }

    }
}