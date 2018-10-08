using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LaDOSE.Entity.Wordpress
{
    public class WPEvent
    {
        [Key]
        // Id, Name, Slug, Date
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Date { get; set; }

        public virtual IEnumerable<WPBooking> WPBookings { get; set; }
    }
}