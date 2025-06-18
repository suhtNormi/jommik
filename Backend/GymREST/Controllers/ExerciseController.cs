using GymREST.Data.Repos;
using GymREST.Models.Classes;
using Microsoft.AspNetCore.Mvc;

namespace GymREST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExerciseController(ExercisesRepo repo): ControllerBase()
    {
        private readonly ExercisesRepo repo = repo;


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var exercises = await repo.GetAllExercises();

                var result = exercises.Select(ex => new ExerciseDto
                {
                    Id = ex.Id,
                    Name = ex.Name,
                    Sets = ex.Sets,
                    Reps = ex.Reps
                }).ToList();

        return Ok(result);
        }

        [HttpGet("bodypart")]
        public async Task<IActionResult> GetExercisesByBodypart(string bodypart){
            var exercises = await repo.GetBodypartExercise(bodypart);
            return Ok(exercises);
        }
    }
}