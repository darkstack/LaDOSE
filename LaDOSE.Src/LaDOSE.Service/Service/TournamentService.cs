using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaDOSE.Business.Interface;
using LaDOSE.Entity.Challonge;
using LaDOSE.Entity.Context;
using LaDOSE.Entity.Wordpress;
using Microsoft.EntityFrameworkCore.Internal;

namespace LaDOSE.Business.Service
{
    public class TournamentService : ITournamentService
    {
        private class Rules
        {
            public int PlayerMin { get; set; }
            public int PlayerMax { get; set; }
            public int FirstPoint { get; set; }
            public int SecondPoint { get; set; }
            public int ThirdFourthPoint { get; set; }
            public int Top8Point { get; set; }
            public int Top16Point { get; set; }

            public Rules(int playerMin, int playerMax, int firstPoint, int secondPoint, int thirdFourthPoint, int top8Point, int top16Point)
            {
                PlayerMin = playerMin;
                PlayerMax = playerMax;
                FirstPoint = firstPoint;
                SecondPoint = secondPoint;
                ThirdFourthPoint = thirdFourthPoint;
                Top8Point = top8Point;
                Top16Point = top16Point;
            }
        }

        private LaDOSEDbContext _context;
        private IChallongeProvider _challongeProvider;

        private List<Rules> TournamentRules = new List<Rules>()
        {
            new Rules(0,8,5,3,2,0,0),
            new Rules(8,16,8,5,3,2,0),
            new Rules(16,32,12,8,5,3,2),
            new Rules(32,Int32.MaxValue,18,12,8,5,3),
        };

        public TournamentService(LaDOSEDbContext context, IChallongeProvider challongeProvider)
        {
            this._context = context;
            this._challongeProvider = challongeProvider;
        }
        public async Task<List<Tournament>> GetTournaments(DateTime? start, DateTime? end)
        {
            List<WPUser> wpUsers = _context.WPUser.ToList();
            var tournaments = await _challongeProvider.GetTournaments(start, end);

            foreach (var tournament in tournaments)
            {
                List<Participent> participents = await _challongeProvider.GetParticipents(tournament.Id);
                tournament.Participents = participents;
            }
            return tournaments;
        }

        public async Task<TournamentsResult> GetTournamentsResult(List<int> ids)
        {
            TournamentsResult result = new TournamentsResult();
            result.Results = new List<Result>();
            var tournaments = new List<Tournament>();
            foreach (var idTournament in ids)
            {
                var tournament = await _challongeProvider.GetTournament(idTournament);
                tournament.Participents = await _challongeProvider.GetParticipents(tournament.Id);
                tournaments.Add(tournament);
            }

            var games = _context.Game.ToList();
            var players = _context.WPUser.ToList();
            
            var allParticipent = tournaments.SelectMany(e => e.Participents).Distinct((a, b) => a.Name == b.Name).ToList();
            foreach (var participent in allParticipent)
            {
                var player = players.FirstOrDefault(e => e.Name.Contains(participent.Name));
                if (player!=null)
                {
                    participent.IsMember = true;
                }

            }

            result.Participents = allParticipent;
            
            foreach (var tournament in tournaments)
            {
                var game = games.First(g => tournament.Name.Contains(g.Name));
                tournament.Game = game;

                var playerCount = tournament.Participents.Count;
                var currentRule = TournamentRules.FirstOrDefault(rules =>
                    rules.PlayerMin < playerCount && rules.PlayerMax >= playerCount
                );
                if (currentRule == null)
                {
                    throw new Exception("Unable to find rules");
                }

                var first = tournament.Participents.First(p => p.Rank == 1);
                var second = tournament.Participents.First(p => p.Rank == 2);
                var thirdFourth = tournament.Participents.Where(p => p.Rank == 3 || p.Rank == 4).ToList();
                var Top8 = tournament.Participents.Where(p => p.Rank >4 && p.Rank<9).ToList();
                var Top16 = tournament.Participents.Where(p => p.Rank >9 && p.Rank<=16).ToList();
                
                result.Results.Add(new Result(first.Name,tournament.Game.Id,currentRule.FirstPoint));
                result.Results.Add(new Result(second.Name,tournament.Game.Id,currentRule.SecondPoint));
                thirdFourth.ForEach(r=> result.Results.Add(new Result(r.Name, tournament.Game.Id, currentRule.ThirdFourthPoint)));
                Top8.ForEach(r=> result.Results.Add(new Result(r.Name, tournament.Game.Id, currentRule.Top8Point)));
                Top16.ForEach(r=> result.Results.Add(new Result(r.Name, tournament.Game.Id, currentRule.Top16Point)));
            }

            result.Games = tournaments.Select(e => e.Game).ToList();

            return result;
        }
    }
}