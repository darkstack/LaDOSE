using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChallongeCSharpDriver.Core.Results;
using LaDOSE.Entity;
using LaDOSE.Entity.Challonge;

namespace LaDOSE.Business.Interface
{
    public interface IChallongeProvider
    {
        Task<string> GetLastTournament();
        string GetLastTournamentMessage();
        Task<TournamentResult> CreateTournament(string name, string url, DateTime? startAt);
        Task<ParticipantResult> AddPlayer(int tournamentId, string userName);

        Task<List<ChallongeTournament>> GetTournaments(DateTime? start, DateTime? end);
        Task<List<ChallongeParticipent>> GetParticipents(int idTournament);
        Task<ChallongeTournament> GetTournament(int idTournament);
        Task<ChallongeTournament> GetTournament(string urlTournament);

        Task<List<Event>> ParseEvent(List<int> idTournaments);


    }

}