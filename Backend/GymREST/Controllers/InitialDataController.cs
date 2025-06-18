using System.Text.Json;
using GymREST.Data.Repos;
using GymREST.Models.Classes;
using Microsoft.AspNetCore.Mvc;

namespace GymREST.Controllers;
[ApiController]
[Route("api/[controller]")]
public class InitialDataController(InitialDatasRepo repo) : ControllerBase() {
    private readonly InitialDatasRepo repo = repo;
    
    [HttpGet]
    public async Task<IActionResult> GetInitialData() {
        var userData = await repo.GetUserData();
        if (userData.Count == 0) return Ok(new List<InitialData>());
        return Ok(userData);
    }

    [HttpPost]
    public async Task<ActionResult> SaveInitialData([FromBody] InitialData initialData) {
        if (initialData == null) {
            return BadRequest("Pole andmeid");
        }
        if (!string.IsNullOrEmpty(initialData.ProfileImageUrl)) {
            initialData.ProfileImageUrl = initialData.ProfileImageUrl;
        }

        var savedData = await repo.SaveUserToDb(initialData);
        return CreatedAtAction(nameof(GetInitialData), new {id = savedData.Id}, savedData);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateInitialData([FromQuery] int userId ,[FromBody] InitialData updatedData) {
        if (updatedData == null) {
            return BadRequest("Pole uusi andmeid");
        }
        if (!string.IsNullOrEmpty(updatedData.ProfileImageUrl)) {
            updatedData.ProfileImageUrl = updatedData.ProfileImageUrl;
        }
        var result = await repo.UpdateUserData(userId ,updatedData);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteInitialData() {
        await repo.DeleteUserData();
        return NoContent();
    }
}
