using GymREST.Data.Repos;
using GymREST.Models.Classes;
using Microsoft.AspNetCore.Mvc;

namespace GymREST.Controllers;
[ApiController]
[Route("api/[controller]")]
public class EventController(EventsRepo repo) : ControllerBase {
    private readonly EventsRepo repo = repo;
    [HttpGet]
        public async Task<IActionResult> GetEvents() {
            var events = await repo.GetAllEventsAsync();
            return Ok(events);
        }

        // GET: api/event/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetEvent(int id)
        {
            var eventItem = await repo.GetEventByIdAsync(id);

            if (eventItem == null)
            {
                return NotFound(new { message = "Sündmust ei leitud." });
            }
            return Ok(eventItem);
        }

        // POST: api/event
        [HttpPost]
        public async Task<ActionResult> CreateEvent([FromBody] Event newEvent)
        {
            if (newEvent == null || string.IsNullOrEmpty(newEvent.Title))
            {
                return BadRequest(new { message = "Pealkiri ja ajad on kohustuslikud." });
            }
            var createdEvent = await repo.AddEventAsync(newEvent);
            return CreatedAtAction(nameof(GetEvent), new { id = createdEvent.Id }, createdEvent);
        }

        // PUT: api/event/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEvent(int id, [FromBody] Event updatedEvent)
        {
            if (id != updatedEvent.Id)
            {
                return BadRequest(new { message = "ID ei ühti." });
            }

            var existingEvent = await repo.GetEventByIdAsync(id);
            if (existingEvent == null)
            {
                return NotFound(new { message = "Sündmust ei leitud." });
            }

            await repo.UpdateEventAsync(updatedEvent);
            return Ok(updatedEvent);
        }

        // DELETE: api/event/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEvent(int id)
        {
            var success = await repo.DeleteEventAsync(id);
            if (!success)
            {
                return NotFound(new { message = "Sündmust ei leitud." });
            }

            return NoContent();
        }

}
