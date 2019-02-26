﻿using System;
using System.Threading.Tasks;
using ChallongeCSharpDriver.Core.Results;

namespace LaDOSE.Business.Interface
{
    public interface IChallongeProvider
    {
        Task<Boolean> GetLastTournament();
        string GetLastTournamentMessage();
        Task<TournamentResult> CreateTournament(string name, string url);
        Task<ParticipantResult> AddPlayer(int tournamentId, string userName);
    }
}