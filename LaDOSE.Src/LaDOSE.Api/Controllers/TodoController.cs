using LaDOSE.Business.Interface;
using LaDOSE.DTO;
using LaDOSE.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaDOSE.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TodoController : GenericControllerDTO<ITodoService, Todo, TodoDTO>
    {
        public TodoController(ITodoService service) : base(service)
        {

        }



    }
}