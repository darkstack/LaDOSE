
using System;
using System.Collections.Generic;

namespace LaDOSE.DTO
{
    public class WPEvent
    {

        // Id, Name, Slug, Date
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public DateTime? Date { get; set; }
        public List<WPBooking> WpBookings { get; set; }
    }

    public class WPBooking
    {
        public WPUser WpUser { get; set; }
        public string Message { get; set; }

        public string Meta { get; set; }
    }

    public class WPUser
    {
        public string Name { get; set; }
    }

}