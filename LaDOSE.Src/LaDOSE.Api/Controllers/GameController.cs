using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LaDOSE.Business.Interface;
using LaDOSE.DTO;
using LaDOSE.Entity;
using LaDOSE.Entity.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaDOSE.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class GameController : GenericControllerDTO<IGameService, Game, GameDTO>
    {
        public GameController(IMapper mapper,IGameService service) : base(mapper,service)
        {
        }
    }
}
