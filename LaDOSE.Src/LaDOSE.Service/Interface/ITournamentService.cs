using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LaDOSE.Entity.Challonge;

namespace LaDOSE.Business.Interface
{
    public interface ITournamentService
    {
        Task<List<ChallongeTournament>> GetTournaments(DateTime? start, DateTime? end);

        Task<TournamentsResult> GetTournamentsResult(List<int> ids);
    }
}