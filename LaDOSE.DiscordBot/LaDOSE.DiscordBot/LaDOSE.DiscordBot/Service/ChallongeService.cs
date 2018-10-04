using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChallongeCSharpDriver;
using ChallongeCSharpDriver.Caller;
using ChallongeCSharpDriver.Core.Queries;
using ChallongeCSharpDriver.Core.Results;
using ChallongeCSharpDriver.Main;
using ChallongeCSharpDriver.Main.Objects;

namespace LaDOSE.DiscordBot.Service
{
    public class ChallongeService
    {
        private ChallongeConfig Config;
        public string ApiKey { get; set; }

        public ChallongeHTTPClientAPICaller ApiCaller { get; set; }

        public string DernierTournois { get; set; }


        public ChallongeService(string apiKey)
        {
            this.ApiKey = apiKey;
            this.Config = new ChallongeConfig(this.ApiKey);
            this.ApiCaller = new ChallongeHTTPClientAPICaller(Config);
            DernierTournois = "Aucun tournois.";
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
            var lastRankingDate = new DateTime(lastDate.Year, lastDate.Month, lastDate.Day);
            var lastTournament = tournamentResultList.Where(e => e.completed_at > lastRankingDate).ToList();
            string returnValue = "Les derniers tournois : \n";
            foreach (var tournamentResult in lastTournament)
            {
                returnValue += $"{tournamentResult.name} : <https://challonge.com/{tournamentResult.url}> \n";
            }

            DernierTournois = returnValue;
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