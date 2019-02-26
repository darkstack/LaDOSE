﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChallongeCSharpDriver;
using ChallongeCSharpDriver.Caller;
using ChallongeCSharpDriver.Core.Objects;
using ChallongeCSharpDriver.Core.Queries;
using ChallongeCSharpDriver.Core.Results;
using LaDOSE.Business.Interface;

namespace LaDOSE.Business.Provider
{
    public class ChallongeProvider : IChallongeProvider
    {
        private ChallongeConfig Config;
        public string ApiKey { get; set; }

        public ChallongeHTTPClientAPICaller ApiCaller { get; set; }

        public string DernierTournois { get; set; }


        public ChallongeProvider(string apiKey)
        {
            this.ApiKey = apiKey;
            this.Config = new ChallongeConfig(this.ApiKey);
            this.ApiCaller = new ChallongeHTTPClientAPICaller(Config);
            DernierTournois = "Aucun tournois.";
        }

        public async Task<TournamentResult> CreateTournament(string name, string url)
        {
            var result = await new CreateTournamentQuery(name, TournamentType.Double_Elimination, url).call(ApiCaller);
            return result;


        }

        public async Task<ParticipantResult> AddPlayer(int tournamentId, string userName)
        {
            var p = new ParticipantEntry(userName);
            var result = await new AddParticipantQuery(tournamentId, p).call(ApiCaller);
            return result;

        }

        public async Task<Boolean> GetLastTournament()
        {
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

                    DernierTournois = returnValue;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public string GetLastTournamentMessage()
        {
            return DernierTournois;
        }
    }
}