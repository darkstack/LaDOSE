namespace LaDOSE.Entity.Wordpress
{
    public class WPBooking
    {

        //# WPEventId, WPUserId, Message, Meta
        public int WPEventId { get; set; }

        public WPEvent WPEvent { get; set; }
        public int WPUserId { get; set; }
        public WPUser WPUser { get; set; }
        public string Message { get; set; }
        
        public string Meta { get; set; }

    }
}