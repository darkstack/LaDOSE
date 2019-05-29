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
   
        private ITournamentService _service;
        // GET
        public TournamentController(ITournamentService service)
        {
       
            _service = service;
        }

        [HttpGet("GetTournaments")]
        public async Task<List<TournamentDTO>> GetChallonges()
        {

            var tournaments = await _service.GetTournaments(DateTime.Now.AddMonths(-2), null);
            return AutoMapper.Mapper.Map<List<TournamentDTO>>(tournaments);
            return null;
        }


        [HttpPost("GetResults")]
        public async Task<TournamentsResultDTO> GetResults([FromBody] List<int> ids)
        {
            if (ids == null)
            {
                throw new Exception("Invalid arguments");
            }

            var tournamentsResult = await _service.GetTournamentsResult(ids);
            return AutoMapper.Mapper.Map<TournamentsResultDTO>(tournamentsResult);
            
        }
    }
}