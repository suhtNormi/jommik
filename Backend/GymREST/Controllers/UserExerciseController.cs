using GymREST.Data.Repos;
using GymREST.Models.Classes;
using Microsoft.AspNetCore.Mvc;

namespace GymREST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserExerciseController : ControllerBase
    {
        private readonly ExercisesRepo _exercisesRepo;

        public UserExerciseController(ExercisesRepo exercisesRepo)
        {
            _exercisesRepo = exercisesRepo;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserExercise>>> GetUserExercises()
        {
            var userExercises = await _exercisesRepo.GetAllUserExercises();
            return Ok(userExercises);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<UserExercise>> GetUserExercise(int id)
        {
            // Check if UserExercise exists
            if (!await _exercisesRepo.UserExerciseExistsInDb(id))
            {
                return NotFound();
            }

            // Fetch the UserExercise by id
            var userExercise = await _exercisesRepo.GetUserExerciseById(id);
            return Ok(userExercise);
        }

        [HttpPost]
        public async Task<ActionResult<UserExercise>> CreateUserExercise(UserExercise userExercise)
        {

            var savedUserExercise = await _exercisesRepo.SaveUserExerciseToDb(userExercise);
            return CreatedAtAction(nameof(GetUserExercise), new { id = savedUserExercise.Id }, savedUserExercise);
        }
    }
}
