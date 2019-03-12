﻿using System.Collections.Generic;
using LaDOSE.Business.Interface;
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

        public WordPressController(IWordPressService service, IGameService gameService )
        {
            GameService = gameService;
            _service = service;
        }

 

        [HttpGet("WPEvent")]
        public List<WPEvent> Event()
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
            return wpEvents;
        }


        [HttpGet("GetUsers/{wpEventId}/{gameId}")]
        public List<WPUser> GetUsers(int wpEventId, int gameId)
        {
            var game = GameService.GetById(gameId);
            return _service.GetBooking(wpEventId, game);

        }
        [HttpGet("GetUsersOptions/{wpEventId}/{gameId}")]
        public List<WPUser> GetUsersOptions(int wpEventId, int gameId)
        {
            var game = GameService.GetById(gameId);
            return _service.GetBookingOptions(wpEventId, game);

        }


        [HttpGet("UpdateDb")]
        public bool UpdateDb()
        {
            return _service.UpdateBooking();
            
        }

        [HttpGet("CreateChallonge/{gameId:int}/{wpEventId:int}")]
        public string CreateChallonge(int gameId, int wpEventId)
        {
            return _service.CreateChallonge(gameId, wpEventId,null);
        }

        [HttpPost("CreateChallonge/{gameId:int}/{wpEventId:int}")]
        public string CreateChallonge(int gameId, int wpEventId, [FromBody]List<WPUser> additionalPlayer)
        {
            return _service.CreateChallonge(gameId, wpEventId, additionalPlayer);
        }
    }
}