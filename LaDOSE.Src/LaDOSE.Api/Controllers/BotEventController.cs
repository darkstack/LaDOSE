using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LaDOSE.Business.Interface;
using LaDOSE.DTO;
using LaDOSE.Entity;
using LaDOSE.Entity.BotEvent;
using LaDOSE.Entity.Wordpress;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LaDOSE.Api.Controllers
{
    [AllowAnonymous]
    //[Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class BotEventController : GenericControllerDTO<IBotEventService, BotEvent, BotEventDTO>
    {
     
        public BotEventController(IMapper mapper,IBotEventService service) : base(mapper,service)
        {
     
        }

        [Route("CreateBotEvent/{eventName}")]
        public bool CreateBotEvent(String eventName)
        {
            return this._service.CreateEvent(eventName);
        }
        [HttpPost]
        [Route("ResultBotEvent/")]
        public bool ResultBotEvent([FromBody] BotEventSendDTO result)
        {
            return this._service.SetResult(result.DiscordId,result.DiscordName,result.Present);

        }

        [Route("GetLastBotEvent/")]
        public BotEventDTO GetLastBotEvent()
        {
            return this._mapper.Map<BotEventDTO>(this._service.GetLastEvent());
        }
    }
}