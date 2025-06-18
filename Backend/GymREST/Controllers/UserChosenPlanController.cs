using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymREST.Data;
using GymREST.Data.Repos;
using GymREST.Models.Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymREST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserChosenPlanController(UserChosenPlanRepo userChosenPlanRepo) : ControllerBase
    {
      private readonly UserChosenPlanRepo _repo = userChosenPlanRepo;
      


    [HttpGet]
     public async Task<IActionResult> GetAllUserChosenPlans(){
        var plans = await _repo.GetUserChosenPlans();
        return Ok(plans);
    }

    [HttpGet("PlanName")]
    public async Task<IActionResult> GetPlanName(int userId){
        var planName = await _repo.GetUserChosenPlanName(userId);
        if (planName == null){
            return NotFound("this user has no plan");
        }
        return Ok(planName);
    }



    [HttpGet("{userId}")]
    public async Task<ActionResult<List<UserChosenPlan>>> GetPlanByUserId(int userId)
    {
       var plans = await _repo.GetUserChosenPlanByUserId(userId);
        return Ok(plans);
    }

    [HttpGet("{userId}/exercises")]
    public async Task<IActionResult> GetAllUserChosenExercises(int userId){
        var userExists = await _repo.UserExistsInDb(userId);
        if(!userExists){
            return BadRequest("no such user in db");
        }
        var exercises = await _repo.GetAllUserChosenPlanExercises(userId);
        return Ok(exercises);
    }


    [HttpPost("create")]
    public async Task<IActionResult> CreateUserChosenPlan([FromQuery] int userId, [FromQuery] int planId)
    {
        if (userId <= 0 || planId <= 0)
        {
            return BadRequest(new { message = "Invalid UserId or PlanId" });
        }

        try
        {
            var userChosenPlan = await _repo.CreateUserChosenPlan(userId, planId);

            return CreatedAtAction(nameof(CreateUserChosenPlan), new { id = userChosenPlan.Id }, userChosenPlan);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred", details = ex.Message });
        }
    }
    [HttpPut]
    public async Task<IActionResult> UpdateUserChosenPlan([FromQuery] int userId, [FromQuery] int newPlanId){
        if (newPlanId <= 0)
        {
            return BadRequest(new { message = "Invalid PlanId" });
        }

        try {
            var updatedUserChosenPlan = await _repo.ChangeUserPlan(userId, newPlanId);
            return Ok(updatedUserChosenPlan);
        }
        catch(ArgumentException ex){
            return BadRequest(ex.Message);

        }

    }








    }
}