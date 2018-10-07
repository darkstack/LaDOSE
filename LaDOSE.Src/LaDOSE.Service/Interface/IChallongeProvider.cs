using System;
using System.Threading.Tasks;

namespace LaDOSE.Business.Interface
{
    public interface IChallongeProvider
    {
        Task<Boolean> GetLastTournament();
        string GetLastTournamentMessage();
        Task<Tuple<int, string>> CreateTournament(string name, string url);
    }
}