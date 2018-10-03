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
        public string ApiKey { get; set; }


        public ChallongeService(string apiKey)
        {
            this.ApiKey = apiKey;
            
        }

        public async Task<String> GetLastTournament()
        {
            ChallongeConfig config = new ChallongeConfig(this.ApiKey);
            var caller = new ChallongeHTTPClientAPICaller(config);
            var tournaments = new Tournaments(caller);
            List<TournamentResult> tournamentResultList = await new TournamentsQuery()
                {
                    state = TournamentState.Ended
                }
                .call(caller);
            List<StartedTournament> tournamentList = new List<StartedTournament>();
            foreach (TournamentResult result in tournamentResultList)
            {
                tournamentList.Add(new TournamentObject(result, caller));
            }

            var startedTournament = tournamentList.Last();

            return startedTournament.ToString();
        }
    }
}