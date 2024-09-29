using MemLib;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : Controller
    {
        private readonly IMenuService _iMenuService;

        public MenuController(IMenuService menuService)
        {
            _iMenuService = menuService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string json = await _iMenuService.GetMenu();

            return Ok(json);
        }
    }
}
