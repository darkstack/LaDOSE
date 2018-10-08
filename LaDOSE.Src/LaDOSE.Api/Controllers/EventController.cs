using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaDOSE.Business.Interface;
using LaDOSE.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LaDOSE.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class EventController : Controller
    {
        private IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost]
        public Event Post([FromBody]Event dto)
        {
            return _eventService.Create(dto);
        }

        [HttpGet("{id}")]
        public Event Get(int id)
        {
            return _eventService.GetById(id);

        }

        [HttpGet("Generate/{eventId}/{wpEventId}")]
        public bool GenerateChallonge(int eventId, int wpEventId)
        {
             return _eventService.CreateChallonge(eventId, wpEventId);
            
        }
    }
}