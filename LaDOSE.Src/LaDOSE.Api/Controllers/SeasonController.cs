using LaDOSE.Business.Interface;
using LaDOSE.Business.Service;
using LaDOSE.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaDOSE.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class SeasonController : GenericController<ISeasonService,Season>
    {
        public SeasonController(ISeasonService service) : base(service)
        {
        }
    }
}