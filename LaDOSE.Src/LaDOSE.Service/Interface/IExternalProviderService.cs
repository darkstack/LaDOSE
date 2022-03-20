﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LaDOSE.Entity;
using LaDOSE.Entity.Challonge;

namespace LaDOSE.Business.Interface
{
    public interface IExternalProviderService
    {
        Task<List<ChallongeTournament>> GetTournaments(DateTime? start, DateTime? end);

        Task<TournamentsResult> GetTournamentsResult(List<int> ids);
        Task<TournamentsResult> GetSmashResult(string tournamentSlug);

        Task<TournamentsResult> GetSmashResult2(string tournamentSlug);


        Task<List<Event>> GetChallongeEvents(List<int> ids);
    }
}