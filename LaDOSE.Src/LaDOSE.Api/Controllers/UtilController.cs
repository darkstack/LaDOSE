using LaDOSE.Business.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaDOSE.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UtilController : Controller
    {
        private IUtilService _service;

        public UtilController(IUtilService service)
        {
            _service = service;
        }

      

        [HttpGet("UpdateBooking")]
        public bool UpdateBooking()
        {
            return _service.UpdateBooking();

        }
    }
}