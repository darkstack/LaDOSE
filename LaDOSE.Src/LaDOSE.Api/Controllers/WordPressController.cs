using System.Collections.Generic;
using LaDOSE.Business.Interface;
using LaDOSE.Entity.Wordpress;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaDOSE.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class WordPressController : Controller
    {
        // GET

        public WordPressController(IWordPressService service)
        {
            _service = service;
        }

        private IWordPressService _service;

        [HttpGet("WPEvent")]
        public List<WPEvent> Event()
        {
            return _service.GetWpEvent();

        }
    }
}