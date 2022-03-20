using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LaDOSE.Entity;
using LaDOSE.Entity.Challonge;
using LaDOSE.Entity.Wordpress;

namespace LaDOSE.Business.Interface
{
    public interface IEventService : IBaseService<Event>
    {
        Task<List<ChallongeTournament>> GetTournaments(DateTime? start, DateTime? end);

        Task<TournamentsResult> GetTournamentsResult(List<int> ids);
        Task<TournamentsResult> GetSmashResult(string tournamentSlug);

        Task<TournamentsResult> GetSmashResult2(string tournamentSlug);


    }
}