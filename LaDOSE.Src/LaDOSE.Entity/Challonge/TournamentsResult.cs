using System;
using System.Collections.Generic;

namespace LaDOSE.Entity.Challonge
{
    public class TournamentsResult
    {
        public List<ChallongeParticipent> Participents { get; set; }
        public List<Game> Games{ get; set; }

        public List<Result> Results { get; set; }
        public string Slug { get; set; }
    }

    public class Result
    {
       
        public Result(string player, int gameId, int point,int rank) : this(player, gameId, 0,"", point,rank)
        {

        }


        public Result(string player, int gameId, int tournamentdId,string tournamentUrl, int point,int rank)
        {
            Player = player;
            GameId = gameId;
            Point = point;
            TournamentUrl = tournamentUrl;
            TournamentdId = tournamentdId;
            Rank = rank;
        }


        public Result()
        {
        }


        public int TournamentdId { get; set; }
        public string TournamentUrl { get; set; }
        public int GameId { get; set; }
        public string Player { get; set; }
        public int Point { get; set; }
        public int Rank { get; set; }
    }
}