using System;

namespace LaDOSE.DTO
{
    public class TodoDTO
    {
        public int Id { get; set; }
        public string User { get; set; }
        public string Task { get; set; }
        public bool Done { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Deleted { get; set; }

    }
}