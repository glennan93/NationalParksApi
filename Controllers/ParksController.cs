using Microsoft.AspNetCore.Mvc;
using NationalParksApi.Models;
using NationalParksApi.Repositories;

namespace NationalParksApi.Controllers
{
    [ApiController]
    [Route("parks")]
    public class ParksController: ControllerBase
    {
        private readonly IParkRepository _repo;

        public ParksController(IParkRepository repo)
        {
            _repo = repo;
        }

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
        public IActionResult CreatePark([FromBody] NationalPark newPark)
        {
            var created = _repo.Add(newPark);
            return CreatedAtAction(nameof(GetParks), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePark(int id, [FromBody] NationalPark updatedPark)
        {
            var park = _repo.Update(id, updatedPark);
            if (park == null)
                return NotFound("Park not found.");
            return Ok(park);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePark(int id)
        {
            var success = _repo.Delete(id);
            if (!success)
                return NotFound("Park not found.");
            return Ok($"Park with ID {id} deleted.");
        }
    }
}