using MyWebApp1.Data;
using MyWebApp1.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MyWebApp1.Models.MyWebApp1.Models;
using MyWebApp1.DTO;
using System.Data;
using Microsoft.EntityFrameworkCore;
using MyWebApp1.Payload.Response;

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

        public string Register(RegisterUserDTO userDTO)
        {
            try
            {
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

                // Gán role mặc định là "user"
                var userRole = new UserRole
                {
                    UserId = newUser.UserId,
                    RoleId = _dbContext.Roles.FirstOrDefault(r => r.RoleName == "user").RoleId
                };
                _dbContext.UserRoles.Add(userRole);
                _dbContext.SaveChanges();

                return "User registered successfully.";
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                Console.WriteLine(ex.Message);
                return $"An error occurred: {ex.Message}";
            }
        }

        public LoginResponseDTO Login(LoginDTO loginDTO)
        {
            if (string.IsNullOrEmpty(loginDTO.Email) || string.IsNullOrEmpty(loginDTO.Password))
            {
                throw new Exception("Email and Password are required.");
            }

            var user = _dbContext.Users.FirstOrDefault(x => x.Email == loginDTO.Email && x.Password == loginDTO.Password);

            if (user == null)
            {
                throw new Exception("Invalid credentials.");
            }

            // Get the user role
            var userRole = _dbContext.UserRoles.FirstOrDefault(ur => ur.UserId == user.UserId);
            if (userRole == null)
            {
                throw new Exception("User role not found.");
            }

            var role = _dbContext.Roles.FirstOrDefault(r => r.RoleId == userRole.RoleId);
            if (role == null)
            {
                throw new Exception("Role not found for this user.");
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId", user.UserId.ToString()),
                new Claim("Email", user.Email),
                new Claim("Role", role.RoleName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenExpiration = DateTime.UtcNow.AddMinutes(60);
            var token = new JwtSecurityToken(
                _configuration["Jwt:issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: tokenExpiration,
                signingCredentials: signIn
            );

            return new LoginResponseDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserId = user.UserId,
                Email = user.Email,
                Fullname = user.Fullname,
                Username = user.Username,
                RoleName = role.RoleName,
                TokenExpiration = tokenExpiration
            };
        }

        //public async Task<CurrentUserDTO> GetCurrentUserLogin(ClaimsPrincipal currentUser)
        //{
        //    try
        //    {
        //        // Lấy UserId từ token
        //        var currentUserId = int.Parse(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);

        //        // Tìm người dùng dựa trên UserId
        //        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == currentUserId);
        //        if (user == null)
        //        {
        //            throw new Exception("User not found.");
        //        }

        //        // Tạo DTO để trả về thông tin người dùng
        //        return new CurrentUserDTO
        //        {
        //            UserId = user.UserId,
        //            Fullname = user.Fullname,
        //            Username = user.Username,
        //            Email = user.Email,
        //            PhoneNumber = user.PhoneNumber,
        //            Address = user.Address
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"An error occurred: {ex.Message}");
        //    }
        //}

        public async Task<LoginResponseDTO> UpdateProfile(int userId, UpdateProfileDTO userDTO, ClaimsPrincipal currentUser)
        {
            try
            {
                // Lấy UserId từ token
                var currentUserId = int.Parse(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);

                // Chỉ cho phép chỉnh sửa chính thông tin của người dùng
                if (currentUserId != userId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to update this profile.");
                }

                // Tìm người dùng cần cập nhật
                var userToUpdate = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserId == userId);
                if (userToUpdate == null)
                {
                    throw new Exception("User not found.");
                }

                // Kiểm tra xem Username mới có bị trùng với người khác không
                if (!string.IsNullOrEmpty(userDTO.Username) &&
                    _dbContext.Users.Any(u => u.Username == userDTO.Username && u.UserId != userId))
                {
                    throw new Exception("Username already exists.");
                }

                // Cập nhật các trường chỉ khi có giá trị
                userToUpdate.Username = !string.IsNullOrEmpty(userDTO.Username) ? userDTO.Username : userToUpdate.Username;
                userToUpdate.Fullname = !string.IsNullOrEmpty(userDTO.Fullname) ? userDTO.Fullname : userToUpdate.Fullname;
                userToUpdate.Email = !string.IsNullOrEmpty(userDTO.Email) ? userDTO.Email : userToUpdate.Email;
                userToUpdate.PhoneNumber = !string.IsNullOrEmpty(userDTO.PhoneNumber) ? userDTO.PhoneNumber : userToUpdate.PhoneNumber;
                userToUpdate.Address = !string.IsNullOrEmpty(userDTO.Address) ? userDTO.Address : userToUpdate.Address;

                // Cập nhật mật khẩu nếu có giá trị mới
                if (!string.IsNullOrEmpty(userDTO.Password))
                {
                    userToUpdate.Password = userDTO.Password;
                }

                // Lưu thay đổi vào database
                _dbContext.Users.Update(userToUpdate);
                await _dbContext.SaveChangesAsync();

                // Lấy vai trò người dùng
                var userRole = await _dbContext.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userToUpdate.UserId);
                var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.RoleId == userRole.RoleId);

                // Tạo token JWT mới
                var tokenExpiration = DateTime.UtcNow.AddMinutes(60);
                var claims = new[]
                {
            new Claim("UserId", userToUpdate.UserId.ToString()),
            new Claim("Email", userToUpdate.Email),
            new Claim("Role", role.RoleName)
        };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: tokenExpiration,
                    signingCredentials: signIn
                );

                // Trả về LoginResponseDTO chứa thông tin người dùng sau khi cập nhật
                return new LoginResponseDTO
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    UserId = userToUpdate.UserId,
                    Email = userToUpdate.Email,
                    Fullname = userToUpdate.Fullname,
                    Username = userToUpdate.Username,
                    RoleName = role.RoleName,
                    TokenExpiration = tokenExpiration
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}");
            }
        }


        public async Task<User> RequestManagerRole(string requestedUsername, string loggedInUsername)
        {
            // Check if the username in the request matches the logged-in user's username.
            if (requestedUsername != loggedInUsername)
            {
                throw new Exception("You can only request a role for your own account.");
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == requestedUsername);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            // Check if the user has already requested the manager role.
            if (user.IsApprovedUser)
            {
                throw new Exception("User has already requested the manager role.");
            }

            // Mark the user as having requested the "manager" role.
            user.IsApprovedUser = true;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<Models.Pet> AddNewPet(Models.Pet pet)
        {
            // Tạo pet mới
            var Addpet = new Models.Pet
            {
                PetName = pet.PetName,
                PetType = pet.PetType,
                Age = pet.Age,
                Gender = pet.Gender,
                Address = pet.Address,
                MedicalCondition = pet.MedicalCondition,
                ContactPhoneNumber = pet.ContactPhoneNumber,
                ContactEmail = pet.ContactEmail,
                PetCategoryId = pet.PetCategoryId,
                IsAdopted = false,
                IsApproved = false
            };

            // Thêm vào db
            await _dbContext.Pets.AddAsync(Addpet);
            await _dbContext.SaveChangesAsync();

            return Addpet;
        }

        public async Task<List<Pet>> GetAllApprovedPets()
        {
            // Lọc các pet có isApproved = true và isAdopted = false
            return await _dbContext.Pets
                .Where(pet => pet.IsApproved == true && pet.IsAdopted == false)
                .ToListAsync();
        }


        public async Task<PetResponse> GetApprovedPet(int petId)
        {
            // Chỉ trả về pet có isApproved = true
            var pet = await _dbContext.Pets
                .FirstOrDefaultAsync(pet => pet.PetId == petId && pet.IsApproved == true);

            // Kiểm tra nếu không tìm thấy pet hoặc pet chưa được duyệt
            if (pet == null)
            {
                return null; // Hoặc trả về giá trị phù hợp để báo lỗi không tìm thấy pet
            }

            return new PetResponse()
            {
                PetId = pet.PetId,
                PetName = pet.PetName,
                Address = pet.Address,
                MedicalCondition = pet.MedicalCondition,
                ContactPhoneNumber = pet.ContactPhoneNumber,
                ContactEmail = pet.ContactEmail,
                Age = pet.Age,
                Gender = pet.Gender,
                PetType = pet.PetType,
                IsAdopted = pet.IsAdopted,
                IsApproved = pet.IsApproved
            };
        }
    }
}
