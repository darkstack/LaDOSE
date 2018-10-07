using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LaDOSE.Entity
{
    public class Season
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual IEnumerable<SeasonGame> Games { get; set; }

        public virtual IEnumerable<Event> Event { get; set; }
    }
}