
using System;
using System.Collections.Generic;

namespace LaDOSE.DTO
{
    public class WPEventDTO
    {

        // Id, Name, Slug, Date
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public DateTime? Date { get; set; }
        public List<WPBookingDTO> WpBookings { get; set; }
    }
}