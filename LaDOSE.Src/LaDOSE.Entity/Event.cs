﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LaDOSE.Entity
{
    public class Event : Context.Entity
    {
        //[Key]
        //public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int SeasonId { get; set; }
        public Season Season { get; set; }
        public bool Ranking { get; set; }


        public virtual IEnumerable<EventGame> Games { get; set; }


    }
}