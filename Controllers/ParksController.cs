using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NationalParksApi.Models;
using NationalParksApi.Repositories;

namespace NationalParksApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ParksController: ControllerBase
    {
        private readonly IParkRepository _repo;
        private readonly IWeatherService _weatherService;

        public ParksController(IParkRepository repo, IWeatherService weatherService)
        {
            _repo = repo;
            _weatherService = weatherService;
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        public IActionResult GetParks([FromQuery] string? name, [FromQuery] int? year, [FromQuery] string? state, [FromQuery] int? id)
        {
            var parks = _repo.GetAll();

            if (!string.IsNullOrWhiteSpace(name))
                parks = parks.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

            if (year.HasValue)
                parks = parks.Where(p => p.YearEstablished == year.Value);

            if (!string.IsNullOrWhiteSpace(state))
                parks = parks.Where(p => p.State.Contains(state, StringComparison.OrdinalIgnoreCase));

            if (id.HasValue)
            {
                var park = _repo.GetById(id.Value);
                if (park == null)
                    return NotFound("Park not found.");
                return Ok(park);
            }

            return Ok(parks);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IActionResult CreatePark([FromBody] NationalPark newPark)
        {
            var created = _repo.Add(newPark);
            return CreatedAtAction(nameof(GetParks), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]

        public IActionResult UpdatePark(int id, [FromBody] NationalPark updatedPark)
        {
            var park = _repo.Update(id, updatedPark);
            if (park == null)
                return NotFound("Park not found.");
            return Ok(park);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]

        public IActionResult DeletePark(int id)
        {
            var success = _repo.Delete(id);
            if (!success)
                return NotFound("Park not found.");
            return Ok($"Park with ID {id} deleted.");
        }

        [HttpGet("{id}/currentweather")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetParkWeather(int id)
        {
            var park = _repo.GetById(id);
            var name = park.Name;
            if (park == null)
                return NotFound("Park not found.");

            var weatherData = await _weatherService.GetCurrentWeatherAsync(park.Latitude, park.Longitude);
            if (weatherData == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to retrieve weather data.");

            var parkWeatherInfo = new ParkWeatherInfo
            {
                ParkName = name,
                WeatherData = weatherData
            };

            return Ok(parkWeatherInfo);
        }
    }
}