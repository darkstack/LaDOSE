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
        private IExternalProviderService provider;
        public GameController(IMapper mapper,IGameService service, IExternalProviderService service2) : base(mapper,service)
        {
            provider = service2;
        }
        [HttpGet("smash/{name}")]
        public async Task<List<GameDTO>> GetIdFromSmash(string name)
        {
            var smashGame = await provider.GetSmashGame(name);
            
            return _mapper.Map<List<GameDTO>>(smashGame);;
        }
    }
}
