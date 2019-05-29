using System;
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
        public int Rank { get; set; }
        public bool? IsMember{ get; set; }
    }

    public class TournamentsResultDTO
    {
        public List<ParticipentDTO> Participents { get; set; }
        public List<GameDTO> Games { get; set; }

        public List<ResultDTO> Results { get; set; }
    }
    public class ResultDTO
    {
        public int GameId { get; set; }
        public string Player { get; set; }
        public int Point { get; set; }
    }
}