using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymREST.Data.Repos;
using GymREST.Models.Classes;
using Microsoft.AspNetCore.Mvc;

namespace GymREST.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class WorkoutEventController(WorkoutEventRepo repo):ControllerBase
    {
        private readonly WorkoutEventRepo repo = repo;


        [HttpPost("create")]
        public async Task<IActionResult> CreateEvent([FromBody] WorkoutEvent eventData){
            if (eventData == null)
            {
                return BadRequest("invalid sisestus");
            }
            var newEvent = await repo.CreateEvent(eventData.UserId, eventData);
            return CreatedAtAction(nameof(CreateEvent), new {id = newEvent.Id}, newEvent);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] WorkoutEvent eventData){
            if (id != eventData.Id)
            {
                return BadRequest("ids dont match");
            }
            var existingExercise = await repo.GetEventById(id);
            if (existingExercise == null){
                return NotFound("no such exercise with that id");
            }
            var updatedExercise = await repo.UpdateWorkoutEvent(eventData);
            return Ok(updatedExercise);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEvents(){
            var events = await repo.GetAllEvents();
            return Ok(events);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetEventByName(string name){
            var workoutEvent = await repo.GetEventByName(name);
            if (workoutEvent == null){
                return NotFound("no events found");
            }
            return Ok(workoutEvent);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id){
            var deleted = await repo.DeleteWorkoutEvent(id);
            if(!deleted){
                return NotFound("no event with such id");
            }
            return NoContent();
        }



    

    }
}