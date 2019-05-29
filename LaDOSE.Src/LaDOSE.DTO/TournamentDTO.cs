using System.Collections.Generic;

namespace LaDOSE.DTO
{
    public class TournamentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Game { get; set; }
        public List<ParticipentDTO> Participents {get;set;}
    }

    public class ParticipentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Rank { get; set; }
        public bool? IsMember{ get; set; }
    }
}