using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaDOSE.Business.Interface;
using LaDOSE.Entity;
using LaDOSE.Entity.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaDOSE.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class GameController : ControllerBase
    {

        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }
        // GET api/Game
        [HttpGet]
        public List<Game> Get()
        {

            return _gameService.GetAll().ToList();

        }
  
        // GET api/Game/5
        [HttpGet("{id}")]
        public Game Get(int id)
        {
            return _gameService.GetById(id);
        }

        [HttpPut()]
        public bool Put(Game game)
        {
            return _gameService.Update(game);
        }

    }
}
