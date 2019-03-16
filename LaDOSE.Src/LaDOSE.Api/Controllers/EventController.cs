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

    
    }
}