using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaDOSE.Api.Context;
using LaDOSE.Entity;
using Microsoft.AspNetCore.Mvc;

namespace LaDOSE.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {

        private readonly LaDOSEDbContext _db;

        public ConfigController(LaDOSEDbContext db)
        {
            _db = db;
        }
        // GET api/Config
        [HttpGet]
        public ActionResult<IEnumerable<Game>> Get()
        {
   
                return _db.Game.ToList();
           
        }

        // GET api/Config/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }
    }
}
