using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GymREST.Data.Repos;
using GymREST.Models.Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GymREST.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class CalendarExerciseController(CalendarExerciseRepo repo) : ControllerBase
    {
        private readonly CalendarExerciseRepo repo = repo;

        [HttpPost]
        public async Task<IActionResult> CreateExercise ([FromBody]CalendarExercise exerciseData){
            if (exerciseData == null)
            {
                return BadRequest("invalid sisestus");
            }
            var exercise = await repo.CreateCalendarExercise(exerciseData);
            return CreatedAtAction(nameof(CreateExercise), new {id = exercise.Id}, exercise);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExercise (int id, [FromBody] CalendarExercise updateExercise){
             if (id != updateExercise.Id)
            {
                return BadRequest("ids dont match");
            }
            var existingExercise = await repo.GetCalendarExerciseById(id);
            if (existingExercise == null){
                return NotFound("no such exercise with that id");
            }
            var updatedExercise = await repo.UpdateCalendarExercise(updateExercise);
            return Ok(updatedExercise);

        }
        [HttpGet]
        public async Task<IActionResult> GetAllExercises(){
            var exercises = await repo.GetAllCalendarExercises();
            return Ok(exercises);
        }
        [HttpGet("{name}")]
        public async Task<IActionResult> GetExercisesByName(string name){
            var exercises = await repo.GetExercisesByName(name);
            if (exercises == null){
                return NotFound("no exercises exist with that name");
            }
            return Ok(exercises);
        }
//silueti sisendandmed
        [HttpGet("exerciseData")]
        public async Task<IActionResult> GetExerciseData(int userId, string from, string to){
            try
            {
                var groupedExercises = await repo.GetExerciseBodypartData(userId, from, to);
                return Ok(groupedExercises);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}