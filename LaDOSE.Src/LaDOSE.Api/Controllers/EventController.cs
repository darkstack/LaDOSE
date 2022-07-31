using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LaDOSE.Business.Interface;
using LaDOSE.DTO;
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
    public class EventController : GenericControllerDTO<IEventService, Event, EventDTO>
    {
        private IGameService _gameService;

        public EventController(IMapper mapper, IEventService service,IGameService gameService) : base(mapper,service)
        {
            this._gameService = gameService;
        }


        public override List<EventDTO> Get()
        {
            return this._mapper.Map<List<EventDTO>>(_service.GetAll().OrderByDescending(e=>e.Date).ToList());
        }

    }
}