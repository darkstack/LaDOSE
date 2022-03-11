using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LaDOSE.Business.Provider.SmashProvider;
using LaDOSE.Entity.Challonge;

namespace LaDOSE.Business.Interface
{
    public interface ISmashProvider
    {
        Task<List<ChallongeTournament>> GetTournaments(DateTime? start, DateTime? end);
        Task<ResponseType> GetTournament(string sludge);

    }
}