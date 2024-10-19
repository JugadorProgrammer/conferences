using Microsoft.AspNetCore.Mvc;

namespace Conference.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConnectionController : ControllerBase
    {
        [HttpGet]
        [Route("[action]")]
        public IActionResult ConnectToRoom()
        {

            return Ok();
        }
    }
}
