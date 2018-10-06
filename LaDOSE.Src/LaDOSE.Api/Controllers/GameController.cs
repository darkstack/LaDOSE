using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        private readonly LaDOSEDbContext _db;

        public GameController(LaDOSEDbContext db)
        {
            _db = db;
        }
        // GET api/Config
        [HttpGet]
        public List<Game> Get()
        {
   
                return _db.Game.ToList();
           
        }
  
        // GET api/Config/5
        [HttpGet("{id}")]
        public Game Get(int id)
        {
            return _db.Game.FirstOrDefault(e=>e.Id==id);
        }
 
      
    }
}
