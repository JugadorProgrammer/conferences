using Conference.Core.Interfaces;
using Conference.Core.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Conference.Core.DTOModels;

namespace Conference.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IDataBaseService _dataBaseService;

        public AuthenticationController(IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        [HttpPost]
        [Route("[action]")] 
        public async Task<IActionResult> SingIn([FromBody] SingInModel singInModel)
        {
            var user = await _dataBaseService.GetUser(singInModel.Email, singInModel.Password);
            if (user is null)
            {
                return BadRequest("User not found!");
            }

            await Authenticate(user);
            return Accepted();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> SingUp(User user)
        {
            if (await _dataBaseService.ExistUser(user))
            {
                return BadRequest("User already created!");
            }

            user.Id = await _dataBaseService.CreateUser(user);
            await Authenticate(user);
            return Created();
        }

        [HttpPost]
        [Authorize]
        [Route("[action]")]
        public new async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return Accepted();
        }

        [NonAction]
        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString())
            };

            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
