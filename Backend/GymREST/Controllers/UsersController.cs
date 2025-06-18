using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymREST.Data;
using GymREST.Models.Classes;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GymREST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        private IConfiguration _config;

        public UsersController(IConfiguration config, DataContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpGet("last")]
        public async Task<IActionResult> GetLastUser(){
            var lastUser = await _context.Users
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync();
            if (lastUser == null)
            {
                return NotFound("No users found.");
            }
            return Ok(lastUser);
        }

        [HttpPost("login")]
        public IActionResult Login(User login)
        {
            var dbUser = _context.Users!.FirstOrDefault(user => user.Username == login.Username);

            if (dbUser == null) return NotFound();

            if (dbUser.Password != HashPassword(login.Password)) return Unauthorized();

            var token = GenerateJSONWebToken(dbUser);

            return Ok(new { token });
        }

        [HttpPost("createAccount")]
        public IActionResult CreateAccount(User createUser)
        {
            var dbUser=_context.Users!.FirstOrDefault(x=>x.Username==createUser.Username);
            

            if(dbUser!=null) return BadRequest();

            createUser.Password=HashPassword(createUser.Password);
            _context.Add(createUser);
            _context.SaveChanges();
            return Ok(createUser);
        }

        private string HashPassword(string password)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: new byte[0],
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

        private string GenerateJSONWebToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]??string.Empty));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              expires: DateTime.UtcNow.AddMinutes(10), //now utcnow asemel
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}