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
        //This may be a get , but i dont know what the RFC State for Get request with Body 
        //As i don't like to populate GET request with body this will be a post (and i think
        //it will be easier to proxy. 
        [HttpPost("GetTournaments")]
        public async Task<List<TournamentDTO>> GetChallonges([FromBody] TimeRangeDTO dto)
        {
            if (dto.To.HasValue | dto.From.HasValue)
            {
                var tournaments = await _service.GetTournaments(dto.From, dto.To);
                return AutoMapper.Mapper.Map<List<TournamentDTO>>(tournaments);
            }
            
            
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