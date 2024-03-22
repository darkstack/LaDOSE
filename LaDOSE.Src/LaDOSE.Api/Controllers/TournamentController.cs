using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
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
   
        private IExternalProviderService _service;

        private IMapper _mapper;

        // GETawa
        public TournamentController(IMapper mapper, IExternalProviderService service)
        {
            _mapper = mapper;
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
                return _mapper.Map<List<TournamentDTO>>(tournaments);
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

            var test = await _service.GetEventsResult(ids);
            return _mapper.Map<TournamentsResultDTO>(test);
            
        }


        [HttpGet("ParseSmash/{tournamentSlug}")]
        public async Task<bool> AddSmashTournament(string tournamentSlug)
        {
            if (!String.IsNullOrEmpty(tournamentSlug))
            {
                var smash = await _service.ParseSmash(tournamentSlug);
                //var tournaments = await _service.GetSmashResult2(tournamentSlug);
                return smash != null;
                //return Ok(tournaments);
            }

            return false;
        }
        [HttpPost("ParseChallonge")]
        public async Task<bool> ParseChallonge([FromBody] List<int> ids)
        {
            if (ids != null)
            {
                var tournaments = await _service.GetChallongeEvents(ids);
                return tournaments.Count>0;
            }
            return false;
        }

        [AllowAnonymous]
        [HttpGet("GetPlayers/{slug}")]
        public async Task<List<String>> GetPlayer(string slug)
        {
            if (!string.IsNullOrEmpty(slug))
            {
                var player = await _service.GetPlayer(slug);
                return player;
            }

            throw new Exception("Erreur");

        }



    }
}