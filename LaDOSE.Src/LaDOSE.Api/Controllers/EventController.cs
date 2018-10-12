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
    public class EventController : GenericController<IEventService, Event>
    {
        public EventController(IEventService service) : base(service)
        {
        }
    
 
        [HttpGet("Generate/{eventId}/{wpEventId}")]
        public bool GenerateChallonge(int eventId, int wpEventId)
        {
             return _service.CreateChallonge(eventId, wpEventId);
            
        }
    }
}