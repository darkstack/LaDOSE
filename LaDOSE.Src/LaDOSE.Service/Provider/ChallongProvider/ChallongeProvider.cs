using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ChallongeCSharpDriver;
using ChallongeCSharpDriver.Caller;
using ChallongeCSharpDriver.Core.Objects;
using ChallongeCSharpDriver.Core.Queries;
using ChallongeCSharpDriver.Core.Results;
using LaDOSE.Business.Interface;
using LaDOSE.Entity;
using LaDOSE.Entity.Challonge;

namespace LaDOSE.Business.Provider.ChallongProvider
{
    public class ChallongeProvider : IChallongeProvider
    {
        private ChallongeConfig Config;
        public string ApiKey { get; set; }

        public ChallongeHTTPClientAPICaller ApiCaller { get; set; }

        public string DernierTournois { get; set; }

        public ChallongeProvider(IGameService gameService, IEventService eventService, IPlayerService playerService, string apiKey)
        {
            this.ApiKey = apiKey;
            this.Config = new ChallongeConfig(this.ApiKey);
            this.ApiCaller = new ChallongeHTTPClientAPICaller(Config);
            this.EventService = eventService;
            this.GameService = gameService;
            this.PlayerService = playerService;

            DernierTournois = "Aucun tournois.";
        }

        public IPlayerService PlayerService { get; set; }

        public IGameService GameService { get; set; }

        public IEventService EventService { get; set; }

        #region Old Provider 
        public async Task<TournamentResult> CreateTournament(string name, string url, DateTime? startAt = null)
        {
            var result = await new CreateTournamentQuery(name, startAt, TournamentType.Double_Elimination, url).call(ApiCaller);
            return result;


        }

        public async Task<ParticipantResult> AddPlayer(int tournamentId, string userName)
        {
            var p = new ParticipantEntry(userName);
            var result = await new AddParticipantQuery(tournamentId, p).call(ApiCaller);
            return result;

        }

        public async Task<List<ChallongeTournament>> GetTournaments(DateTime? start, DateTime? end)
        {

            List<TournamentResult> tournamentResultList = await new TournamentsQuery()
            {
                state = TournamentState.Ended,
                createdAfter = start,
                createdBefore = DateTime.Now,


            }
                .call(this.ApiCaller);
            List<ChallongeTournament> tournaments = new List<ChallongeTournament>();
            tournamentResultList.ForEach(w => tournaments.Add(new ChallongeTournament()
            {
                ChallongeId = w.id,
                Name = w.name,
                Participents = new List<ChallongeParticipent>()
            }));
            return tournaments;
        }

        public async Task<List<ChallongeParticipent>> GetParticipents(int idTournament)
        {
            var participentResults = await new ParticipantsQuery() { tournamentID = idTournament }.call(ApiCaller);

            List<ChallongeParticipent> participants = new List<ChallongeParticipent>();
            participentResults.ForEach(w =>
            {
                if (w.active)
                {
                    participants.Add(new ChallongeParticipent()
                    {
                        ChallongeTournamentId = idTournament,
                        ChallongeId = w.id,
                        Name = w.name,
                        Rank = w.final_rank,
                        IsMember = false,
                    });
                }

            });
            return participants;
        }

        public async Task<ChallongeTournament> GetTournament(int idTournament)
        {

            var tournamentResult = await new TournamentQuery(idTournament).call(ApiCaller);

            return new ChallongeTournament()
            {
                ChallongeId = tournamentResult.id,
                Name = tournamentResult.name,
                Url = tournamentResult.url,
                Participents = new List<ChallongeParticipent>()

            };

        }
        public async Task<ChallongeTournament> GetTournament(string urlTournament)
        {

            var tournamentResult = await new TournamentQuery(urlTournament).call(ApiCaller);

            return new ChallongeTournament()
            {
                ChallongeId = tournamentResult.id,
                Name = tournamentResult.name,
                Url = tournamentResult.url,
                Participents = new List<ChallongeParticipent>()

            };

        }
        public async Task<string> GetLastTournament()
        {
            string dernierTournois = null;
            try
            {


                List<TournamentResult> tournamentResultList = await new TournamentsQuery()
                {
                    state = TournamentState.Ended
                }
                    .call(this.ApiCaller);


                var lastDate = tournamentResultList.Max(e => e.completed_at);

                if (lastDate.HasValue)
                {
                    var lastRankingDate = new DateTime(lastDate.Value.Year, lastDate.Value.Month, lastDate.Value.Day);

                    var lastTournament = tournamentResultList.Where(e => e.completed_at > lastRankingDate).ToList();
                    string returnValue = "Les derniers tournois : \n";
                    foreach (var tournamentResult in lastTournament)
                    {
                        returnValue += $"{tournamentResult.name} : <https://challonge.com/{tournamentResult.url}> \n";
                    }

                    dernierTournois = returnValue;
                }
                return dernierTournois;
            }
            catch
            {
                return dernierTournois;
            }
        }
        public string GetLastTournamentMessage()
        {
            return DernierTournois;
        }
        #endregion


        public Event TryGetEvent(string eventName, string date)
        {

            var currentevent = this.EventService.GetByName(eventName);
            if (currentevent != null) return currentevent;
            
            var Date = new DateTime(1950, 1, 1);
            //
            try
            {
                Date = DateTime.ParseExact(date, "dd/MM/yy",
                    CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                //Don't care
            }
            currentevent = new Event()
            {
                Name = eventName,
                Date = Date,
            };
            this.EventService.AddOrUpdate(currentevent);

            return currentevent;
        }

        private const string RegexRanking = @"[R|r]anking.?#\w{3}";
        private const string DateRanking = @"^\[(\d{2}\/\d{2}\/\d{2})\]";
        private const string GameRanking = @"\-.(\w*)$";
        public async Task<List<Event>> GetEvents(List<int> idTournaments)
        {
            var result = new List<Event>();
            foreach (var idTournament in idTournaments)
            {
                Task<ChallongeTournament> tournament;
                try
                {
                    tournament = GetTournament(idTournament);
                }
                catch
                {
                    continue;
                    
                }

                if (tournament.Result.Name.Contains("Ranking"))
                {
                    var eventName = Regex.Match(tournament.Result.Name, RegexRanking);
                    var eventDate = Regex.Match(tournament.Result.Name, DateRanking);
                    var tournamentGame = Regex.Match(tournament.Result.Name, GameRanking);


                    if (eventName.Groups.Count > 0 && eventDate.Groups.Count > 1)
                    {
                        var eventNameCapture = eventName.Groups[0].Value;
                        var eventDateCapture = eventDate.Groups[1].Value;

                        var currentevent = result.FirstOrDefault(e => e.Name == eventNameCapture);
                        if (currentevent == null)
                        {
                            currentevent = TryGetEvent(eventNameCapture, eventDateCapture);
                            result.Add(currentevent);
                        }

                        string eventGame = tournament.Result.Name;
                        if (tournamentGame.Groups.Count > 1)
                        {
                            eventGame = tournamentGame.Groups[1].Value;
                        }

                        if (currentevent.Tournaments == null)
                        {
                            currentevent.Tournaments = new List<Tournament>();
                        }
                        var currentTournament = currentevent.Tournaments.FirstOrDefault(e => e.Name.Contains($"Ranking {eventGame}"));
                        if (currentTournament == null)
                        {
                            currentTournament = new Tournament($"Ranking {eventGame}", tournament.Result.ChallongeId, null)
                            {
                                GameId = GameService.GetIdByName(eventGame)
                            };
                            List<ChallongeParticipent> participents = new List<ChallongeParticipent>();
                            try
                            {
                                participents = await GetParticipents(tournament.Result.ChallongeId);
                            }
                            catch
                            {
                                continue;
                            }
                            var results = participents.Select(e => new Entity.Result()
                            {
                                Tournament = currentTournament,
                                PlayerId = this.PlayerService.GetIdByName(e),
                                Rank = e.Rank ?? 999
                            }).ToList();

                            currentTournament.Results = results;
                            currentevent.Tournaments.Add(currentTournament);
                        }

                    }

                }
            }
            return result;
        }
    }
}