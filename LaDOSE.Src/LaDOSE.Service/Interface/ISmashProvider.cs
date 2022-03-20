using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LaDOSE.Business.Provider.SmashProvider;
using LaDOSE.Entity;
using LaDOSE.Entity.Challonge;

namespace LaDOSE.Business.Interface
{
    public interface ISmashProvider
    {
        Task<Event> GetEvent(string slug);
        Task<List<Tournament>> GetResults(ref List<Tournament> tournaments);
        Task<List<Tournament>> GetSets(ref List<Tournament> tournaments);

        Task<Event> ParseEvent(string slug);
        Task<TournamentResponse> GetTournament(string sludge);


        
    }
}