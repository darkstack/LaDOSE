using System;

namespace LaDOSE.Entity
{
    public class Todo : Context.Entity
    {
        public string User { get; set; }
        public string Task { get; set; }
        public bool Done { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Deleted { get; set; }

    }
}