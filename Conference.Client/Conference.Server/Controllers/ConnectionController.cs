using Conference.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conference.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConnectionController : ControllerBase
    {
        private readonly IDataBaseService _dataBaseService;
        private readonly ILogger<ConnectionController> _logger; // Use NLog in the future

        public ConnectionController(IDataBaseService dataBaseService, ILogger<ConnectionController> logger)
        {
            _logger = logger;
            _dataBaseService = dataBaseService;
        }

        [HttpGet]
        [Authorize]
        [Route("[action]/{groupName}")]
        public async Task<IActionResult> GetGroups(string? groupName)
        {
            var groups = await _dataBaseService.GetGroups(groupName);
            return Ok(groups);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult ConnectToRoom()
        {

            return Ok();
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Disconnect()
        {

            return Ok();
        }
    }
}
