using MyWebApp1.Data;
using MyWebApp1.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MyWebApp1.Models.MyWebApp1.Entities;

namespace MyWebApp1.Services
{
    public class UserService
    {
        private readonly MyDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public UserService(MyDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public string Register(UserDTO userDTO)
        {
            try
            {
                // Kiểm tra xem Email hoặc Username đã tồn tại hay chưa
                if (_dbContext.Users.Any(u => u.Email == userDTO.Email || u.Username == userDTO.Username))
                {
                    return "Username or Email already exists.";
                }

                var newUser = new User
                {
                    Username = userDTO.Username,
                    Fullname = userDTO.Fullname,
                    Email = userDTO.Email,
                    PhoneNumber = userDTO.PhoneNumber,
                    Address = userDTO.Address,
                    Password = userDTO.Password,
                    IsApprovedUser = false,
                    IsApproved = false,
                    CreatedAt = DateTime.Now
                };

                _dbContext.Users.Add(newUser);
                _dbContext.SaveChanges();

                return "User registered successfully.";
            }
            catch (Exception ex)
            {
                // Ghi log lỗi (nên sử dụng framework ghi log)
                Console.WriteLine(ex.Message);
                return $"An error occurred: {ex.Message}";
            }
        }

        public string Login(Login loginDTO)
        {
            // Tìm người dùng theo Email và mật khẩu
            var user = _dbContext.Users.FirstOrDefault(x => x.Email == loginDTO.Email && x.Password == loginDTO.Password); // Băm mật khẩu tại đây
            if (user == null)
            {
                throw new Exception("Invalid credentials.");
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId", user.UserId.ToString()),
                new Claim("Email", user.Email.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: signIn
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public List<User> GetUsers()
        {
            return _dbContext.Users.ToList();
        }

        public User GetUser(int id)
        {
            return _dbContext.Users.FirstOrDefault(x => x.UserId == id);
        }

        public string UpdateProfile(int userId, UserDTO userDTO)
        {
            var userToUpdate = _dbContext.Users.FirstOrDefault(x => x.UserId == userId);
            if (userToUpdate == null)
            {
                throw new Exception("User not found.");
            }

            userToUpdate.Fullname = userDTO.Fullname;  // Cập nhật Fullname
            userToUpdate.Email = userDTO.Email;
            userToUpdate.PhoneNumber = userDTO.PhoneNumber;
            userToUpdate.Address = userDTO.Address;

            // Cập nhật mật khẩu nếu có
            if (!string.IsNullOrEmpty(userDTO.Password))
            {
                userToUpdate.Password = userDTO.Password; // Lưu mật khẩu mà không băm
            }

            _dbContext.Users.Update(userToUpdate);
            _dbContext.SaveChanges();

            return "User profile updated successfully.";
        }
    }
}
