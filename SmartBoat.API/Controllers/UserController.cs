using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBoat.Infrastructure.Services.Users;
using SmartBoat.Shared.Authentication;

namespace SmartBoat.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET api/user/userdata
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<ActionResult> UserData()
        {
            var userData = await _userService.UserData(User);
            return Ok(userData);
        }

        // POST api/user/register
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var rootData = await _userService.Register(model);

                    return Created("api/v1/authentication/register", rootData);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);

                }
            }
            string errorMessage = string.Join(", ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return BadRequest(errorMessage ?? "Bad Request");
        }


        // POST api/user/login
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var rootData = await _userService.Login(model);
                    return Ok(rootData);
                }
                catch (Exception ex)
                {
                    return StatusCode((int)HttpStatusCode.Unauthorized, ex.Message);
                }

            }
            string errorMessage = string.Join(", ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return BadRequest(errorMessage ?? "Bad Request");
        }
    }
}
