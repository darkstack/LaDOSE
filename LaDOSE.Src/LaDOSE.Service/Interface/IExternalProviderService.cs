using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LaDOSE.Entity;
using LaDOSE.Entity.Challonge;

namespace LaDOSE.Business.Interface
{
    public interface IExternalProviderService
    {
        Task<List<ChallongeTournament>> GetTournaments(DateTime? start, DateTime? end);
        Task<Event> ParseSmash(string tournamentSlug);
        Task<List<Event>> ParseChallonge(List<int> ids);

        //Task<TournamentsResult> GetChallongeTournamentsResult(List<int> ids);
        //Task<TournamentsResult> GetSmashResult(string tournamentSlug);

        Task<List<Event>> GetChallongeEvents(List<int> ids);

        Task<TournamentsResult> GetEventsResult(List<int> ids);
    }
}