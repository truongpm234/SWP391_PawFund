using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyWebApp1.Data;
using MyWebApp1.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyWebApp1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MyDbContext dbContext;
        private readonly IConfiguration configuration;
        public UsersController(MyDbContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("Registration")]
        public IActionResult Registration(UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var objUser = dbContext.Users.FirstOrDefault(x => x.Email == userDTO.Email);
            if (objUser == null)
            {
                dbContext.Users.Add(new Models.User
                {
                    FirstName = userDTO.FirstName,
                    LastName = userDTO.LastName,
                    Email = userDTO.Email,
                    Password = userDTO.Password,
                });
                dbContext.SaveChanges();
                return Ok("User registered success.");
            }
            else
            {
                return BadRequest("User already exist with same email.");
            }
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginDTO loginDTO)
        {
            var user = dbContext.Users.FirstOrDefault(x => x.Email == loginDTO.Email && x.Password == loginDTO.Password);
            if (user != null)
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserId", user.UserId.ToString()),
                    new Claim("Email", user.Email.ToString()),

                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    configuration["Jwt:issuer"],
                    configuration["Jwt:Audience"],
                    claims,
                    expires : DateTime.UtcNow.AddMinutes(60),
                    signingCredentials: signIn
                    );

                string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new { Token = tokenValue, User = user });


                //return Ok(user);
            }
            return NoContent();
        }

        [HttpGet]
        [Route("GetUsers")]
        public IActionResult GetUsers()
        {
            return Ok(dbContext.Users.ToList());
        }


        //[Authorize]
        [HttpGet]
        [Route("GetUser")]
        public IActionResult GetUser(int id)
        {
            var user = dbContext.Users.FirstOrDefault(x => x.UserId == id);
            if (user != null)
                return Ok(user);
            else
                return NoContent();
        }


        [HttpPut]
        [Route("UpdateProfile")]
        public IActionResult UpdateProfile(int userId, UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userToUpdate = dbContext.Users.FirstOrDefault(x => x.UserId == userId);
            if (userToUpdate == null)
            {
                return NotFound("User not found.");
            }
            // update user
            userToUpdate.FirstName = userDTO.FirstName;
            userToUpdate.LastName = userDTO.LastName;
            userToUpdate.Email = userDTO.Email;
            // update password
            if (!string.IsNullOrEmpty(userDTO.Password))
            {
                userToUpdate.Password = userDTO.Password;
            }
            dbContext.Users.Update(userToUpdate);
            dbContext.SaveChanges();

            return Ok("User profile update success.");
        }
    }

}