using DemoAPIDataAccess;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace DemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private ILoginDA _loginDA;

        public LoginController(IConfiguration config, ILoginDA loginDA) 
        {
            _loginDA = loginDA;
            _config = config;
        }

        [HttpPost]
        [Route("CreateLogin")]
        public async Task<IActionResult> PostNewLogin([FromBody] LoginRequest loginRequest)
        {
            bool success = await _loginDA.CreateLogin(loginRequest.username, loginRequest.password);

            return Ok(success);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> PostLogin([FromBody] LoginRequest loginRequest)
        {
            string token = await _loginDA.GetLogin(loginRequest.username, loginRequest.password);

            if(!string.IsNullOrEmpty(token))
            {
                return Ok(token);
            }

            return Unauthorized();
        }
    }
}
