using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChallongeCSharpDriver.Core.Results;

namespace LaDOSE.Business.Interface
{
    public interface IChallongeProvider
    {
        Task<string> GetLastTournament();
        string GetLastTournamentMessage();
        Task<TournamentResult> CreateTournament(string name, string url);
        Task<ParticipantResult> AddPlayer(int tournamentId, string userName);

        Task<List<TournamentResult>> GetTournaments(DateTime? start, DateTime? end);
    }
}