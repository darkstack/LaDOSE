﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using LaDOSE.Business.Helper;
using LaDOSE.Business.Interface;
using LaDOSE.Business.Provider.SmashProvider;
using LaDOSE.Entity;
using LaDOSE.Entity.Challonge;
using LaDOSE.Entity.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Result = LaDOSE.Entity.Challonge.Result;

namespace LaDOSE.Business.Service
{
    public class ExternalProviderService : IExternalProviderService
    {
        protected LaDOSEDbContext _context;
        private IChallongeProvider _challongeProvider;
        private ISmashProvider _smashProvider;
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



        #endregion

        public ExternalProviderService(LaDOSEDbContext context, IChallongeProvider challongeProvider, ISmashProvider _smashProvider)
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

        //public async Task<TournamentsResult> GetChallongeTournamentsResult(List<int> ids)
        //{
        //    TournamentsResult result = new TournamentsResult();
        //    result.Results = new List<Result>();
        //    var players = _context.WPUser.ToList();
        //    var games = _context.Game.ToList();
        //    var tournaments = await GetChallongeTournaments(ids, games);

        //    var allParticipent = tournaments.SelectMany(e => e.Participents).Distinct((a, b) => a.Name == b.Name)
        //        .ToList();


        //    allParticipent.RemoveAll(e => e.Name.StartsWith("[FORFAIT]"));
        //    //USELESS
        //    //foreach (var participent in allParticipent)
        //    //{
        //    //    var player = players.FirstOrDefault(e => e.Name.Contains(participent.Name));
        //    //    if (player != null)
        //    //    {
        //    //        participent.IsMember = true;
        //    //    }
        //    //}

        //    result.Participents = allParticipent;

        //    foreach (var tournament in tournaments)
        //    {


        //        var playerCount = tournament.Participents.Count;
        //        var lesSacs = tournament.Participents;
        //        var currentRule = TournamentRules.FirstOrDefault(rules =>
        //            rules.PlayerMin < playerCount && rules.PlayerMax >= playerCount
        //        );
        //        if (currentRule == null)
        //        {
        //            throw new Exception("Unable to find rules");
        //        }

        //        var first = tournament.Participents.First(p => p.Rank == 1);
        //        var second = tournament.Participents.First(p => p.Rank == 2);
        //        var thirdFourth = tournament.Participents.Where(p => p.Rank == 3 || p.Rank == 4).ToList();
        //        var Top8 = tournament.Participents.Where(p => p.Rank > 4 && p.Rank < 9).ToList();
        //        var Top16 = tournament.Participents.Where(p => p.Rank > 8 && p.Rank <= 16).ToList();

        //        result.Results.Add(new Result(first.Name, tournament.Game?.Id ?? 0, tournament.ChallongeId, tournament.Url, currentRule.FirstPoint, first.Rank ?? 0));
        //        lesSacs.Remove(first);
        //        result.Results.Add(new Result(second.Name, tournament.Game?.Id ?? 0, tournament.ChallongeId, tournament.Url, currentRule.SecondPoint, second.Rank ?? 0));
        //        lesSacs.Remove(second);
        //        thirdFourth.ForEach(r =>
        //            result.Results.Add(new Result(r.Name, tournament.Game?.Id ?? 0, tournament.ChallongeId, tournament.Url,
        //                currentRule.ThirdFourthPoint, r.Rank ?? 0)));
        //        thirdFourth.ForEach(p => lesSacs.Remove(p));
        //        if (currentRule.Top8Point != 0)
        //        {
        //            Top8.ForEach(r =>
        //                result.Results.Add(new Result(r.Name, tournament.Game?.Id ?? 0, tournament.ChallongeId, tournament.Url, currentRule.Top8Point, r.Rank ?? 0)));
        //            Top8.ForEach(p => lesSacs.Remove(p));
        //        }

        //        if (currentRule.Top16Point != 0)
        //        {
        //            Top16.ForEach(r =>
        //                result.Results.Add(
        //                    new Result(r.Name, tournament.Game?.Id ?? 0, tournament.ChallongeId, tournament.Url, currentRule.Top16Point, r.Rank ?? 0)));
        //            Top16.ForEach(p => lesSacs.Remove(p));
        //        }

        //        lesSacs.ForEach(r =>
        //            result.Results.Add(new Result(r.Name, tournament.Game?.Id ?? 0, tournament.ChallongeId, tournament.Url,
        //                currentRule.Participation, r.Rank ?? 0)));
        //    }

        //    result.Games = tournaments.Select(e => e.Game).Distinct((game, game1) => game.Name == game1.Name).Where(e => e != null).ToList();
        //    if (result.Games == null)
        //    {
        //        result.Games = new List<Game>();
        //    }
        //    result.Games.Add(new Game() { Id = 0, Order = 9999, Name = "UNKNOW" });
        //    return result;
        //}


        public async Task<Event> ParseSmash(string tournamentSlug)
        {
            Event eventExist = GetBySlug(tournamentSlug);
            if (eventExist == null)
            {

                var currentEvent = this._smashProvider.GetEvent(tournamentSlug).Result;
                var tournaments = currentEvent.Tournaments;
                var res = this._smashProvider.GetResults(ref tournaments).Result;
                res = await this._smashProvider.GetSets(ref tournaments);
                this._context.Add(currentEvent);
                try
                {
                    this._context.SaveChanges();
                }
                //POKEMON.
                catch (Exception e)
                {
                    throw new Exception("FUCK !");
                }
                return currentEvent;
            }
            //else
            //{
            //    throw new Exception("Already Exist");
            //}
            //NEED TO UPDATE 
            return eventExist;

        }

        //public Task<List<Event>> ParseChallonge(List<int> ids)
        //{
        //    return GetChallongeEvents(ids);
        //    //return _challongeProvider.Get(ids);
        //}

        private Event GetBySlug(string tournamentSlug)
        {
            return _context.Event.FirstOrDefault(e => e.SmashSlug == tournamentSlug);
        }

        //public async Task<TournamentsResult> GetSmashResult(string tournamentSlug)
        //{
        //    var parse = await this.ParseSmash(tournamentSlug);
        //    var test = await GetEventResult(parse.Id);
        //    return test;

        //}

        //public async Task<TournamentsResult> GetEventsResult(List<int> events)
        //{
        //    var test = await GetEventsResult(events);
        //    return test;
        //}

        public async Task<List<Event>> GetChallongeEvents(List<int> ids)
        {
            var events = await this._challongeProvider.ParseEvent(ids);
            this._context.Event.AddRange(events);
            this._context.SaveChanges();
            return events;
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

        /// <summary>
        /// Get Events Result 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TournamentsResult> GetEventsResult(List<int> id)
        {
            var cevent = new Event();
            cevent.Tournaments = new List<Tournament>();
            List<Player> players = new List<Player>();
            foreach (var eventId in id)
            {
                Event e = _context.Event.Include(e => e.Tournaments).ThenInclude(t => t.Results).ThenInclude(e => e.Player).FirstOrDefault(e => e.Id == eventId);
                players = players.Concat(e.Tournaments.SelectMany(e => e.Results.Select(e => e.Player)).Distinct().ToList()).ToList();
                cevent.Tournaments = cevent.Tournaments.Concat(e.Tournaments).ToList();
            }

            var games = _context.Game.ToList();

            TournamentsResult result = new TournamentsResult();
            result.Results = new List<Result>();
            result.Games = new List<Game>();
            result.Participents = new List<ChallongeParticipent>();
            players.ForEach(e =>
            {
                var x = new ChallongeParticipent()
                {
                    Name = e.Gamertag,
                    ChallongeId = e.Id,
                    Id = e.Id
                };
                result.Participents.Add(x);
            });

            foreach (var tournament in cevent.Tournaments.Where(e => e.Results != null && e.Results.Any()).ToList())
            {

                var tplayer = tournament.Results.Select(e => e.Player).ToList();
                var playerCount = tplayer.Count();
                var lesSacs = tplayer;
                var currentRule = TournamentRules.FirstOrDefault(rules =>
                    rules.PlayerMin < playerCount && rules.PlayerMax >= playerCount
                );
                if (currentRule == null)
                {
                    throw new Exception("Unable to find rules");
                }

                var first = tournament.Results.First(p => p.Rank == 1);
                var second = tournament.Results.First(p => p.Rank == 2);
                var thirdFourth = tournament.Results.Where(p => p.Rank == 3 || p.Rank == 4).ToList();
                var Top8 = tournament.Results.Where(p => p.Rank > 4 && p.Rank < 9).ToList();
                var Top16 = tournament.Results.Where(p => p.Rank > 8 && p.Rank <= 16).ToList();

                result.Results.Add(new Result(first.Player.Gamertag, tournament.Game?.Id ?? 0, tournament.Id, tournament.Name, currentRule.FirstPoint, first.Rank));
                lesSacs.Remove(first.Player);
                result.Results.Add(new Result(second.Player.Gamertag, tournament.Game?.Id ?? 0, tournament.Id, tournament.Name, currentRule.SecondPoint, second.Rank));
                lesSacs.Remove(second.Player);
                thirdFourth.ForEach(r =>
                    result.Results.Add(new Result(r.Player.Gamertag, tournament.Game?.Id ?? 0, tournament.Id, tournament.Name,
                        currentRule.ThirdFourthPoint, r.Rank)));
                thirdFourth.ForEach(p => lesSacs.Remove(p.Player));
                if (currentRule.Top8Point != 0)
                {
                    Top8.ForEach(r =>
                        result.Results.Add(new Result(r.Player.Gamertag, tournament.Game?.Id ?? 0, tournament.Id, tournament.Name,
                            currentRule.Top8Point, r.Rank)));
                    Top8.ForEach(p => lesSacs.Remove(p.Player));
                }

                if (currentRule.Top16Point != 0)
                {
                    Top16.ForEach(r =>
                        result.Results.Add(
                            new Result(r.Player.Gamertag, tournament.Game?.Id ?? 0, tournament.Id, tournament.Name,
                                currentRule.Top16Point, r.Rank)));
                    Top16.ForEach(p => lesSacs.Remove(p.Player));
                }

                lesSacs.ForEach(r =>
                    result.Results.Add(new Result(r.Gamertag, tournament.Game?.Id ?? 0, tournament.Id, tournament.Name,
                        currentRule.Participation, tournament.Results.FirstOrDefault(e => e.Player == r)?.Rank ?? 999)));

            }

            if (result.Results.Any(e => e.GameId == 0))
            {
                result.Games.Add(new Game() { Id = 0, Name = "GAME NOT FOUND", LongName = "GAME NOT FOUND", Order = 999 });
            }

            var enumerable = result.Results.Select(e => e.GameId).Distinct();
            result.Games = _context.Game.Where(g => enumerable.Contains(g.Id)).ToList();

            System.Diagnostics.Trace.WriteLine(result.Results);

            return await Task.FromResult(result);
        }

        public async Task<List<string>> GetPlayer(string slug)
        {
            var tournament = await _smashProvider.GetNames(slug);
            var players = tournament.Tournament.Events.SelectMany(e => e.entrants.nodes.SelectMany(x => x.participants.Select(e => e.gamerTag))).ToList();
            return players;

        }

        /// <summary>
        /// Get Result For one Event
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TournamentsResult> GetEventResult(int id)
        {

            Event cevent = _context.Event.Include(e => e.Tournaments).ThenInclude(t => t.Results).ThenInclude(e => e.Player).FirstOrDefault(e => e.Id == id);
            var players = cevent.Tournaments.SelectMany(e => e.Results.Select(e => e.Player)).Distinct().ToList();
            var games = _context.Game.ToList();

            TournamentsResult result = new TournamentsResult();
            result.Results = new List<Result>();
            result.Games = new List<Game>();
            result.Participents = new List<ChallongeParticipent>();
            players.ForEach(e =>
            {
                var x = new ChallongeParticipent()
                {
                    Name = e.Gamertag,
                    ChallongeId = e.Id,
                    Id = e.Id
                };
                result.Participents.Add(x);
            });

            foreach (var tournament in cevent.Tournaments.Where(e => e.Results != null && e.Results.Any()).ToList())
            {

                var tplayer = tournament.Results.Select(e => e.Player).ToList();
                var playerCount = tplayer.Count();
                var lesSacs = tplayer;
                var currentRule = TournamentRules.FirstOrDefault(rules =>
                    rules.PlayerMin < playerCount && rules.PlayerMax >= playerCount
                );
                if (currentRule == null)
                {
                    throw new Exception("Unable to find rules");
                }

                var first = tournament.Results.First(p => p.Rank == 1);
                var second = tournament.Results.First(p => p.Rank == 2);
                var thirdFourth = tournament.Results.Where(p => p.Rank == 3 || p.Rank == 4).ToList();
                var Top8 = tournament.Results.Where(p => p.Rank > 4 && p.Rank < 9).ToList();
                var Top16 = tournament.Results.Where(p => p.Rank > 8 && p.Rank <= 16).ToList();

                result.Results.Add(new Result(first.Player.Gamertag, tournament.Game?.Id ?? 0, tournament.Id, tournament.Name, currentRule.FirstPoint, first.Rank));
                lesSacs.Remove(first.Player);
                result.Results.Add(new Result(second.Player.Gamertag, tournament.Game?.Id ?? 0, tournament.Id, tournament.Name, currentRule.SecondPoint, second.Rank));
                lesSacs.Remove(second.Player);
                thirdFourth.ForEach(r =>
                    result.Results.Add(new Result(r.Player.Gamertag, tournament.Game?.Id ?? 0, tournament.Id, tournament.Name,
                        currentRule.ThirdFourthPoint, r.Rank)));
                thirdFourth.ForEach(p => lesSacs.Remove(p.Player));
                if (currentRule.Top8Point != 0)
                {
                    Top8.ForEach(r =>
                        result.Results.Add(new Result(r.Player.Gamertag, tournament.Game?.Id ?? 0, tournament.Id, tournament.Name,
                            currentRule.Top8Point, r.Rank)));
                    Top8.ForEach(p => lesSacs.Remove(p.Player));
                }

                if (currentRule.Top16Point != 0)
                {
                    Top16.ForEach(r =>
                        result.Results.Add(
                            new Result(r.Player.Gamertag, tournament.Game?.Id ?? 0, tournament.Id, tournament.Name,
                                currentRule.Top16Point, r.Rank)));
                    Top16.ForEach(p => lesSacs.Remove(p.Player));
                }

                lesSacs.ForEach(r =>
                    result.Results.Add(new Result(r.Gamertag, tournament.Game?.Id ?? 0, tournament.Id, tournament.Name,
                        currentRule.Participation, tournament.Results.FirstOrDefault(e => e.Player == r)?.Rank ?? 999)));

            }

            if (result.Results.Any(e => e.GameId == 0))
            {
                result.Games.Add(new Game() { Id = 0, Name = "GAME NOT FOUND", LongName = "GAME NOT FOUND", Order = 999 });
            }

            var enumerable = result.Results.Select(e => e.GameId).Distinct();
            result.Games = _context.Game.Where(g => enumerable.Contains(g.Id)).ToList();

            System.Diagnostics.Trace.WriteLine(result.Results);

            return await Task.FromResult(result);
        }
    }
}