using Microsoft.AspNetCore.Mvc;

namespace NationalParksApi.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetWelcomeMessage()
        {
            return Ok("Welcome to the National Parks API!");
        }
    }
}