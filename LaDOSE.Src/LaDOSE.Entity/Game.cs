using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LaDOSE.Entity
{
    public class Game
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }

        public virtual IEnumerable<SeasonGame> Seasons { get; set; }
        public virtual IEnumerable<EventGame> Events { get; set; }

    }
}
