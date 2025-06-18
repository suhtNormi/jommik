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
    public class ProfileController(ProfileRepo profileRepo): ControllerBase
    {
        private readonly ProfileRepo _repo = profileRepo;


        [HttpGet]
        public async Task<IActionResult> GetProfiles(){
            var profiles = await _repo.GetAllProfiles();
            return Ok(profiles);
        }

        [HttpGet("last")]
        public async Task<IActionResult> GetLastProfile(){
            var lastProfile = await _repo.GetLastProfile();	
            // if (lastProfile == null)
            // {
            //     return NotFound("No profiles found.");
            // }
            return Ok(lastProfile);
        }

        [HttpGet("initial-data")]
        public async Task<IActionResult> GetInitialDataFromProfile(int userId){
            try{
                var initialData = await _repo.PullInitialDataFromProfile(userId);
                return Ok(initialData);
            }
            catch(Exception ex){
                return NotFound(new { Message = ex.Message });
            }

        }
        [HttpPost("createProfile")]
        public async Task<IActionResult> CreateProfile([FromQuery] int userId, [FromBody] InitialData initialData){
            var profile = await _repo.CreateProfile(userId, initialData);
            return Ok(profile);
        }

        

    }
}