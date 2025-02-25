using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NationalParksApi.Models;
using NationalParksApi.Repositories;
using Microsoft.Extensions.Logging;
using SQLitePCL;

namespace NationalParksApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ParksController : ControllerBase
    {
        private readonly IParkRepository _repo;
        private readonly IWeatherService _weatherService;
        private readonly ILogger<ParksController> _logger;
        private readonly IAuthorizationService _authorizationService;

        public ParksController(IParkRepository repo, IWeatherService weatherService, ILogger<ParksController> logger, IAuthorizationService authorizationService)
        {
            _repo = repo;
            _weatherService = weatherService;
            _logger = logger;
            _authorizationService = authorizationService;
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        public IActionResult GetParks([FromQuery] string? name, [FromQuery] int? year, [FromQuery] string? state, [FromQuery] int? id)
        {
            _logger.LogInformation("Getting parks with filters: name={Name}, year={Year}, state={State}, id={Id}", name, year, state, id);
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
                {
                    _logger.LogWarning("Park with ID {Id} not found.", id.Value);
                    return NotFound("Park not found.");
                }
                return Ok(park);
            }

            return Ok(parks);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public IActionResult CreatePark([FromBody] NationalPark newPark)
        {
            var currentUserEmail = User.Identity?.Name;
            if (!string.IsNullOrEmpty(currentUserEmail))
            {
                newPark.CreatedBy = currentUserEmail;
            }

            _logger.LogInformation("Creating new park: {Name} in {State}, established in {YearEstablished} at (Lat: {Latitude}, Lon: {Longitude}) created by {CreatedBy}.",
            newPark.Name, newPark.State, newPark.YearEstablished, newPark.Latitude, newPark.Longitude, newPark.CreatedBy);

            var created = _repo.Add(newPark);
            return CreatedAtAction(nameof(GetParks), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdatePark(int id, [FromBody] NationalPark updatedPark)
        {
            _logger.LogInformation("Updating park: {Name} in {State}, established in {YearEstablished} at (Lat: {Latitude}, Lon: {Longitude})",
            updatedPark.Name, updatedPark.State, updatedPark.YearEstablished, updatedPark.Latitude, updatedPark.Longitude);
            var existingPark = _repo.GetById(id);
            if (existingPark == null)
            {
                _logger.LogWarning("Park with ID {Id} not found.", id);
                return NotFound("Park not found.");
            }

            var authoriationResult = await _authorizationService.AuthorizeAsync(User, existingPark, "OwnerOrAdmin");
            if (!authoriationResult.Succeeded)
            {
                _logger.LogWarning("User is not authorized to update park with ID {Id}.", id);
                return Forbid();
            }

            var park = _repo.Update(id, updatedPark);
            return Ok(park);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePark(int id)
        {
            _logger.LogInformation("Deleting park with ID {Id}", id);
            var park = _repo.GetById(id);
            if (park == null)
            {
                _logger.LogWarning("Park with ID {Id} not found.", id);
                return NotFound("Park not found.");
            }

            var authoriationResult = await _authorizationService.AuthorizeAsync(User, park, "OwnerOrAdmin");
            if (!authoriationResult.Succeeded)
            {
                _logger.LogWarning("User is not authorized to delete park with ID {Id}.", id);
                return Forbid();
            }
            
            var success = _repo.Delete(id);
            if (!success)
            {
                _logger.LogError("Failed to delete park with ID {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete park.");
            }
            return Ok($"Park with ID {id} deleted.");
        }

        [HttpGet("{id}/currentweather")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetParkWeather(int id)
        {
            _logger.LogInformation("Getting weather for park with ID {Id}", id);
            var park = _repo.GetById(id);
            if (park == null)
            {
                _logger.LogWarning("Park with ID {Id} not found.", id);
                return NotFound("Park not found.");
            }

            var parkName = park.Name;
            var weatherData = await _weatherService.GetCurrentWeatherAsync(park.Latitude, park.Longitude);
            if (weatherData == null)
            {
                _logger.LogError("Unable to retrieve weather data for park with ID {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to retrieve weather data.");
            }

            var parkWeatherInfo = new ParkWeatherInfo
            {
                ParkName = parkName,
                WeatherData = weatherData
            };

            return Ok(parkWeatherInfo);
        }
    }
}
