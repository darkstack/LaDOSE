namespace LaDOSE.Entity
{
    public class EventGame 
    {
  
        public int EventId { get; set; }
        public Event Event { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }

        public int? ChallongeId { get; set; }
        public string ChallongeUrl { get; set; }
    }
}