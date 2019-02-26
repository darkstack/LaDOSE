using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaDOSE.Business.Interface;
using LaDOSE.Entity;
using LaDOSE.Entity.Wordpress;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LaDOSE.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class EventController : GenericController<IEventService, Event>
    {
        private IGameService _gameService;

        public EventController(IEventService service,IGameService gameService) : base(service)
        {
            this._gameService = gameService;
        }

        [HttpGet("GetUsers/{eventId}/{wpEventId}/{gameId}")]
        public List<WPUser> GetUsers(int eventId, int wpEventId,int gameId)
        {
            var game = _gameService.GetById(gameId);
            return _service.GetBooking(eventId, wpEventId,game);

        }

        [HttpGet("Generate/{eventId}/{wpEventId}")]
        public bool GenerateChallonge(int eventId, int wpEventId)
        {
             return _service.CreateChallonge(eventId, wpEventId);
            
        }
    }
}