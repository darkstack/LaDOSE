using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaDOSE.Business.Helper;
using LaDOSE.Business.Interface;
using LaDOSE.Business.Provider.SmashProvider;
using LaDOSE.Entity;
using LaDOSE.Entity.Challonge;
using LaDOSE.Entity.Context;
using LaDOSE.Entity.Wordpress;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace LaDOSE.Business.Service
{
    public class TournamentService : BaseService<ChallongeTournament>,ITournamentService
    {
        private IChallongeProvider _challongeProvider;

        #region Rules
        private class Rules
        {
            public int PlayerMin { get; set; }
            public int PlayerMax { get; set; }
            public int FirstPoint { get; set; }
            public int SecondPoint { get; set; }
            public int ThirdFourthPoint { get; set; }
            public int Top8Point { get; set; }
            public int Top16Point { get; set; }

            public int Participation => 1;

            public Rules(int playerMin, int playerMax, int firstPoint, int secondPoint, int thirdFourthPoint,
                int top8Point, int top16Point)
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

        //Rules Definitions (Min Players,Max Players,First Reward,Second Reward,Third / Fourth Reward, Top 8 reward, Top 16 Reward
        private List<Rules> TournamentRules = new List<Rules>()
        {
            new Rules(0, 8, 5, 3, 2, 0, 0),
            new Rules(8, 16, 8, 5, 3, 2, 0),
            new Rules(16, 32, 12, 8, 5, 3, 2),
            new Rules(32, Int32.MaxValue, 18, 12, 8, 5, 3),
        };

        private ISmashProvider _smashProvider;

        #endregion

        public TournamentService(LaDOSEDbContext context, IChallongeProvider challongeProvider, ISmashProvider _smashProvider) : base(context)
        {
            this._context = context;
            this._challongeProvider = challongeProvider;
            this._smashProvider = _smashProvider;
        }

        public async Task<List<ChallongeTournament>> GetTournaments(DateTime? start, DateTime? end)
         {
            return await _challongeProvider.GetTournaments(start, end);
            //Useless
            //foreach (var tournament in tournaments)
            //{
            //    List<ChallongeParticipent> participents = await _challongeProvider.GetParticipents(tournament.ChallongeId);
            //    tournament.Participents = participents;
            //}
        }

        public async Task<TournamentsResult> GetTournamentsResult(List<int> ids)
        {
            TournamentsResult result = new TournamentsResult();
            result.Results = new List<Result>();
            var players = _context.WPUser.ToList();
            var games = _context.Game.ToList();
            var tournaments = await GetChallongeTournaments(ids,games);

            var allParticipent = tournaments.SelectMany(e => e.Participents).Distinct((a, b) => a.Name == b.Name)
                .ToList();

            
            allParticipent.RemoveAll(e => e.Name.StartsWith("[FORFAIT]"));
            //USELESS
            //foreach (var participent in allParticipent)
            //{
            //    var player = players.FirstOrDefault(e => e.Name.Contains(participent.Name));
            //    if (player != null)
            //    {
            //        participent.IsMember = true;
            //    }
            //}

            result.Participents = allParticipent;

            foreach (var tournament in tournaments)
            {


                var playerCount = tournament.Participents.Count;
                var lesSacs = tournament.Participents;
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
                var Top8 = tournament.Participents.Where(p => p.Rank > 4 && p.Rank < 9).ToList();
                var Top16 = tournament.Participents.Where(p => p.Rank > 8 && p.Rank <= 16).ToList();

                result.Results.Add(new Result(first.Name, tournament.Game.Id, tournament.ChallongeId, tournament.Url, currentRule.FirstPoint,first.Rank??0));
                lesSacs.Remove(first);
                result.Results.Add(new Result(second.Name, tournament.Game.Id, tournament.ChallongeId, tournament.Url, currentRule.SecondPoint, second.Rank ?? 0));
                lesSacs.Remove(second);
                thirdFourth.ForEach(r =>
                    result.Results.Add(new Result(r.Name, tournament.Game.Id, tournament.ChallongeId, tournament.Url,
                        currentRule.ThirdFourthPoint, r.Rank ?? 0)));
                thirdFourth.ForEach(p => lesSacs.Remove(p));
                if (currentRule.Top8Point != 0)
                {
                    Top8.ForEach(r =>
                        result.Results.Add(new Result(r.Name, tournament.Game.Id, tournament.ChallongeId, tournament.Url, currentRule.Top8Point, r.Rank ?? 0)));
                    Top8.ForEach(p => lesSacs.Remove(p));
                }

                if (currentRule.Top16Point != 0)
                {
                    Top16.ForEach(r =>
                        result.Results.Add(
                            new Result(r.Name, tournament.Game.Id, tournament.ChallongeId, tournament.Url, currentRule.Top16Point, r.Rank ?? 0)));
                    Top16.ForEach(p => lesSacs.Remove(p));
                }

                lesSacs.ForEach(r =>
                    result.Results.Add(new Result(r.Name, tournament.Game.Id, tournament.ChallongeId, tournament.Url,
                        currentRule.Participation, r.Rank ?? 0)));
            }

            result.Games = tournaments.Select(e => e.Game).Distinct((game, game1) => game.Name == game1.Name).ToList();

            return result;
        }

        public async Task<TournamentsResult> GetSmashResult(string tournamentSlug)
        {
            var tournaments = await _smashProvider.GetTournament(tournamentSlug);
            var players = tournaments.Tournament.Events.Where(e=>e.standings != null ).SelectMany(e =>  e.standings.nodes.Select(e => e.player)).ToList();
            var distinctp = players.DistinctBy(e=>new {e.user.id}).ToList();
            var games = _context.Game.ToList();

            TournamentsResult result = new TournamentsResult();
            result.Results = new List<Result>();
            result.Games = new List<Game>();
            result.Participents = new List<ChallongeParticipent>();
            distinctp.ForEach(e =>
            {
                var x = new ChallongeParticipent()
                {
                    Name = e.gamerTag,
                    ChallongeId = e.id,
                    Id = e.id
                };
                result.Participents.Add(x);
            });
            games.ForEach(e =>
            {
                e.Id = e.SmashId ?? e.Id;
                result.Games.Add(e);
            });

            foreach (var tournament in tournaments.Tournament.Events.Where(e=>e.standings!=null).ToList())
            {
              

                    var playerCount = tournament.standings.nodes.Count;
                    var lesSacs = tournament.standings.nodes;
                    var currentRule = TournamentRules.FirstOrDefault(rules =>
                        rules.PlayerMin < playerCount && rules.PlayerMax >= playerCount
                    );
                    if (currentRule == null)
                    {
                        throw new Exception("Unable to find rules");
                    }

                    var first = tournament.standings.nodes.First(p => p.placement == 1);
                    var second = tournament.standings.nodes.First(p => p.placement == 2);
                    var thirdFourth = tournament.standings.nodes.Where(p => p.placement == 3 || p.placement == 4).ToList();
                    var Top8 = tournament.standings.nodes.Where(p => p.placement > 4 && p.placement < 9).ToList();
                    var Top16 = tournament.standings.nodes.Where(p => p.placement > 8 && p.placement <= 16).ToList();

                    result.Results.Add(new Result(first.player.gamerTag, tournament.videogame.id, tournament.id, tournament.name, currentRule.FirstPoint, first.placement));
                    lesSacs.Remove(first);
                    result.Results.Add(new Result(second.player.gamerTag, tournament.videogame.id, tournament.id, tournament.name, currentRule.SecondPoint, second.placement));
                    lesSacs.Remove(second);
                    thirdFourth.ForEach(r =>
                        result.Results.Add(new Result(r.player.gamerTag, tournament.videogame.id, tournament.id, tournament.name,
                            currentRule.ThirdFourthPoint, r.placement)));
                    thirdFourth.ForEach(p => lesSacs.Remove(p));
                    if (currentRule.Top8Point != 0)
                    {
                        Top8.ForEach(r =>
                            result.Results.Add(new Result(r.player.gamerTag, tournament.videogame.id, tournament.id, tournament.name,
                                currentRule.ThirdFourthPoint, r.placement)));
                        Top8.ForEach(p => lesSacs.Remove(p));
                    }

                    if (currentRule.Top16Point != 0)
                    {
                        Top16.ForEach(r =>
                            result.Results.Add(
                                new Result(r.player.gamerTag, tournament.videogame.id, tournament.id, tournament.name,
                                    currentRule.ThirdFourthPoint, r.placement)));
                        Top16.ForEach(p => lesSacs.Remove(p));
                    }

                    lesSacs.ForEach(r =>
                        result.Results.Add(new Result(r.player.gamerTag, tournament.videogame.id, tournament.id, tournament.name,
                            currentRule.ThirdFourthPoint, r.placement)));
                
            }



            return await Task.FromResult(result);
        }

        /// <summary>
        /// Check if the tournament exist in database otherwise call Challonge.
        /// </summary>
        /// <param name="ids">tournaments ids</param>
        /// <param name="games">List of known games</param>
        /// <returns>List of the challonge's tournament with participents</returns>
        private async Task<List<ChallongeTournament>> GetChallongeTournaments(List<int> ids, List<Game> games)
        {
            var tournaments = new List<ChallongeTournament>();
            foreach (var idTournament in ids)
            {
                if (!TournamentExist(idTournament))
                {
                    ChallongeTournament challongeTournament = await _challongeProvider.GetTournament(idTournament);
                    challongeTournament.Participents =
                        await _challongeProvider.GetParticipents(challongeTournament.ChallongeId);

                    var game = games.FirstOrDefault(g => challongeTournament.Name.Contains(g.Name));
                    if (game != null) challongeTournament.Game = game;
                    challongeTournament.Sync = DateTime.Now;

                    tournaments.Add(challongeTournament);
                    _context.ChallongeTournament.Add(challongeTournament);
                    _context.SaveChanges();
                }
                else
                {
                    tournaments.Add(_context.ChallongeTournament.Where(e => e.ChallongeId == idTournament)
                        .Include(e => e.Participents).First());
                }
            }

            return tournaments;
        }

        private bool TournamentExist(int idTournament)
        {
            return this._context.ChallongeTournament.Any(e => e.ChallongeId == idTournament);
        }
    }
}