using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LaDOSE.Business.Interface;
using LaDOSE.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaDOSE.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TournamentController : Controller
    {
        public IGameService GameService { get; }

        private IWordPressService _service;
        // GET
        public TournamentController(IWordPressService service, IGameService gameService)
        {
            GameService = gameService;
            _service = service;
        }

        [HttpGet("GetTournaments")]
        public async Task<List<TournamentDTO>> GetChallonges()
        {

            var tournaments = await _service.GetTournaments(DateTime.Now.AddMonths(-2), null);
            return AutoMapper.Mapper.Map<List<TournamentDTO>>(tournaments);
        }
    }
}