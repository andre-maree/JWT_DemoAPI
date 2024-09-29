using MemLib;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BankHolidaysController : ControllerBase
    {
        private IBankHolidaysService serv;
        public BankHolidaysController(IBankHolidaysService serv)
        {
            this.serv = serv;
        }

        [HttpGet]
        [Route("GetBankHolidaysByRegionId")]
        public async Task<IActionResult> GetBankHolidaysByRegionId(int regionId)
        {
            if (regionId >= 1 && regionId <= 3)
            {

                IEnumerable<Models.BankHolidaysFromDb> res = await serv.GetBankHolidays(regionId);

                return Ok(res);
            }

            return BadRequest("Valid input values: 1 for england-and-wales, 2 for scotland, or 3 for northern-ireland");
        }

        [HttpGet]
        [Route("GetBankHolidaysByRegion")]
        public async Task<IActionResult> GetBankHolidaysByRegion(string region)
        {
            int regionId = 0;

            if (region.Equals("england-and-wales"))
            {
                regionId = 1;
            }
            else if (region.Equals("scotland"))
            {
                regionId = 2;
            }
            else if (region.Equals("northern-ireland"))
            {
                regionId = 3;
            }

            if (regionId == 0)
            {
                return BadRequest("Valid input values: england-and-wales, scotland, or northern-ireland");
            }

            IEnumerable<Models.BankHolidaysFromDb> res = await serv.GetBankHolidays(regionId);

            return Ok(res);
        }

        [HttpGet]
        [Route("GetAllBankHolidays")]
        public async Task<IActionResult> GetAllBankHolidays()
        {
            IEnumerable<Models.BankHolidaysFromDbAll> res = await serv.GetBankHolidays();

            return Ok(res);
        }

        [HttpGet]
        [Route("StartBankHolidaysSaveProcess")]
        public async Task<IActionResult> StartBankHolidaysSaveProcess()
        {
            (bool, string) res = await serv.StartBankHolidaysSaveProcess();

            if (res.Item1)
            {
                return Ok(res.Item2);
            }

            return Conflict(res.Item2);
        }
    }
}
