using GymREST.Data.Repos;
using GymREST.Models.Classes;
using Microsoft.AspNetCore.Mvc;

namespace GymREST.Controllers.PlansController;

[ApiController]
[Route("api/[controller]")]
public class PlansController(PlansRepo repo) : ControllerBase()
{
    private readonly PlansRepo repo = repo;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var plans = await repo.GetAllPlansByExercise();

        var result = plans.Select(plan => new PlanDto
        {
            Id = plan.Id,
            Title = plan.Title,
            Goal = plan.Goal,
            Level = plan.Level,
            Description = plan.Description,

            Exercises = plan.PlanExerciseRels.Select(ex => new ExerciseDto
            {
                Id = ex.Exercise.Id,
                Name = ex.Exercise.Name,
                Sets = ex.Exercise.Sets,
                Reps = ex.Exercise.Reps
            }).ToList()
        });

    return Ok(result);
    }
[HttpGet("{id}")]
public async Task<IActionResult> GetPlan(int id)
{
    var plan = await repo.GetPlanById(id);

    if (plan == null)
    {
        return NotFound();
    }

    var result = new PlanDto
    {
        Id = plan.Id,
        Title = plan.Title,
        Goal = plan.Goal,
        Level = plan.Level,
        Description = plan.Description,
        Exercises = plan.PlanExerciseRels.Select(ex => new ExerciseDto
        {
            Id = ex.Exercise.Id,
            Name = ex.Exercise.Name,
            Sets = ex.Exercise.Sets,
            Reps = ex.Exercise.Reps
        }).ToList()
    };

    return Ok(result);
}
    [HttpGet("{planId}/exercises")]
    public async Task<IActionResult> GetAllExercisesFromPLan(int planId){
        var planExists = await repo.PlanExistsInDb(planId);
            if (!planExists)
            {
                return Conflict();
            }
        var exercises = await repo.GetAllExercisesByPlanId(planId);
        return Ok(exercises);
    }

    [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredPlans([FromQuery] string goal, [FromQuery] string level)
        {
            var plans = await repo.GetPlansByGoalAndLevel(goal, level);
            var result = plans.Select(plan => new PlanDto
            {
                Id = plan.Id,
                Title = plan.Title,
                Goal = plan.Goal,
                Level = plan.Level,
                Description = plan.Description,
                Exercises = plan.PlanExerciseRels.Select(ex => new ExerciseDto
                {
                    Id = ex.Exercise.Id,
                    Name = ex.Exercise.Name,
                    Sets = ex.Exercise.Sets,
                    Reps = ex.Exercise.Reps
                }).ToList()
            });

            return Ok(result);
        }
}


