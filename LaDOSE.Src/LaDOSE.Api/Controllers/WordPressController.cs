using System.Collections.Generic;
using AutoMapper;
using LaDOSE.Business.Interface;
using LaDOSE.DTO;
using LaDOSE.Entity.Wordpress;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaDOSE.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class WordPressController : Controller
    {
        public IGameService GameService { get; }

        private IWordPressService _service;
        // GET

        public WordPressController(IWordPressService service, IGameService gameService)
        {
            GameService = gameService;
            _service = service;
        }


        [HttpGet("WPEvent")]
        public List<WPEventDTO> Event()
        {
            var wpEvents = _service.GetWpEvent();
            foreach (var wpEvent in wpEvents)
            {
                foreach (var wpEventWpBooking in wpEvent.WPBookings)
                {
                    wpEventWpBooking.WPEvent = null;
                    wpEventWpBooking.WPUser.WPBookings = null;
                }
            }

            return Mapper.Map<List<WPEventDTO>>(wpEvents);
        }

        
        [HttpGet("NextEvent")]
        public WPEventDTO NextEvent()
        {
            var wpEvents = _service.GetNextWpEvent();


            foreach (var wpEventWpBooking in wpEvents.WPBookings)
            {
                wpEventWpBooking.WPEvent = null;
                wpEventWpBooking.WPUser.WPBookings = null;
            }

            return Mapper.Map<WPEventDTO>(wpEvents);
        }

        [HttpGet("GetUsers/{wpEventId}/{gameId}")]
        public List<WPUserDTO> GetUsers(int wpEventId, int gameId)
        {
            var game = GameService.GetById(gameId);
            return Mapper.Map<List<WPUserDTO>>(_service.GetBooking(wpEventId, game));
        }

        [HttpGet("GetUsersOptions/{wpEventId}/{gameId}")]
        public List<WPUserDTO> GetUsersOptions(int wpEventId, int gameId)
        {
            var game = GameService.GetById(gameId);
            return Mapper.Map<List<WPUserDTO>>(_service.GetBookingOptions(wpEventId, game));
        }


        [HttpGet("UpdateDb")]
        public bool UpdateDb()
        {
            return _service.UpdateBooking();
        }

        [HttpGet("CreateChallonge/{gameId:int}/{wpEventId:int}")]
        public string CreateChallonge(int gameId, int wpEventId)
        {
            return _service.CreateChallonge(gameId, wpEventId, null);
        }

        [HttpPost("CreateChallonge/{gameId:int}/{wpEventId:int}")]
        public string CreateChallonge(int gameId, int wpEventId, [FromBody] List<WPUser> additionalPlayer)
        {
            return _service.CreateChallonge(gameId, wpEventId, additionalPlayer);
        }


    }
}