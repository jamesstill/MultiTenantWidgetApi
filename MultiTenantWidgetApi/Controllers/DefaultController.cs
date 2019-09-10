using Microsoft.AspNetCore.Mvc;

namespace MultiTenantWidgetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Listening...");
        }
    }
}