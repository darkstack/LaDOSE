using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LaDOSE.Entity.Wordpress
{
    public class WPUser
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string WPUserId { get; set; }
        public string WPMail { get; set; }
        public virtual IEnumerable<WPBooking> WPBookings { get; set; }
    }
}