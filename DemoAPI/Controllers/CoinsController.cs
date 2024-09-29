using MemLib;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CoinsController : ControllerBase
    {
        private ICoinStatsService serv;
        private IConfiguration _config;

        public CoinsController(ICoinStatsService serv)
        {
            this.serv = serv;
        }

        [HttpGet]
        public async Task<IActionResult> Get() 
        {
            Models.CoinsRoot? coinsResult = await serv.GetCoinStats();

            return Ok(coinsResult);
        }
    }
}
