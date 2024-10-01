using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApp1.Data;
using MyWebApp1.Models;

namespace MyWebApp1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MyDbContext dbContext;
        public UsersController(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
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
                return Ok(user);
            }
            return NoContent();
        }

        [HttpGet]
        [Route("GetUsers")]
        public IActionResult GetUsers()
        {
            return Ok(dbContext.Users.ToList());
        }

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