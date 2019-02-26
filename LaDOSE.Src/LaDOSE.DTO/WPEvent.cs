
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

    }
}