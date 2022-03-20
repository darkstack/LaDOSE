using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LaDOSE.Business.Interface;
using LaDOSE.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaDOSE.Api.Controllers
{
#if DEBUG
    [AllowAnonymous]
#endif
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class SmashController : Controller
    {

        private IExternalProviderService _service;

        private IMapper _mapper;

        // GET
        public SmashController(IMapper mapper, IExternalProviderService service)
        {
            _mapper = mapper;
            _service = service;
        }
        //This may be a get , but i dont know what the RFC State for Get request with Body 
        //As i don't like to populate GET request with body this will be a post (and i think
        //it will be easier to proxy. 
        [HttpGet("GetTournament/{tournamentSlug}")]
        public async Task<IActionResult> GetSmashTournament(string tournamentSlug)
        {
            if (!String.IsNullOrEmpty(tournamentSlug))
            {
                
                var tournaments = await _service.GetSmashResult(tournamentSlug);

                return Ok(tournaments);
            }

            return null;
        }
        [HttpGet("AddTournament/{tournamentSlug}")]
        public async Task<IActionResult> AddSmashTournament(string tournamentSlug)
        {
            if (!String.IsNullOrEmpty(tournamentSlug))
            {
                var tournaments = await _service.GetSmashResult2(tournamentSlug);

                return Ok(tournaments);
            }

            return null;
        }


    }
}
