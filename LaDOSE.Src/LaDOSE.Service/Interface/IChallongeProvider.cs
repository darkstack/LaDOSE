﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChallongeCSharpDriver.Core.Results;
using LaDOSE.Entity.Challonge;

namespace LaDOSE.Business.Interface
{
    public interface IChallongeProvider
    {
        Task<string> GetLastTournament();
        string GetLastTournamentMessage();
        Task<TournamentResult> CreateTournament(string name, string url, DateTime? startAt);
        Task<ParticipantResult> AddPlayer(int tournamentId, string userName);

        Task<List<Tournament>> GetTournaments(DateTime? start, DateTime? end);
        Task<List<Participent>> GetParticipents(int idTournament);
        Task<Tournament> GetTournament(int idTournament);
        Task<Tournament> GetTournament(string urlTournament);
    }
}