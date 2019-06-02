using System;
using System.Collections.Generic;

namespace LaDOSE.Entity.Challonge
{
    public class TournamentsResult
    {
        public List<Participent> Participents { get; set; }
        public List<Game> Games{ get; set; }

        public List<Result> Results { get; set; }
    }

    public class Result
    {
       
        public Result(string player, int gameId, int point) : this(player, gameId, 0,"", point)
        {

        }


        public Result(string player, int gameId, int tournamentdId,string tournamentUrl, int point)
        {
            Player = player;
            GameId = gameId;
            Point = point;
            TournamentUrl = tournamentUrl;
            TournamentdId = tournamentdId;
            
        }


        public Result()
        {
        }


        public int TournamentdId { get; set; }
        public string TournamentUrl { get; set; }
        public int GameId { get; set; }
        public string Player { get; set; }
        public int Point { get; set; }
    }
}