using JWT_API.DTO;
using JWT_API.Services.AuthenService;
using Microsoft.AspNetCore.Mvc;
using JWT_API.Models;

namespace JWT_API.Controllers
{
    [Route("v1/api/home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IAuthenService _authenService;
        public HomeController (IAuthenService authenService)
        {
            this._authenService = authenService;
        }

        [HttpPost("/register")]
        public IActionResult Register(UserDTO userDTO)
        {
            try
            {
                User user = _authenService.Register(userDTO);
                return Ok(new
                {
                    message = "Create Account Success",
                    data = user
                });
            }
            catch (Exception ex) {
                return StatusCode(500);
            }
        }

        [HttpPost("/login")]
        public IActionResult Login(UserLoginDTO loginDTO) {

            try
            {
                string accsetToken = _authenService.Login(loginDTO);
                return Ok(new
                {
                    token = accsetToken
                });
            }
            catch (Exception ex) {
                return StatusCode(500);
            }
        }

    }
}
