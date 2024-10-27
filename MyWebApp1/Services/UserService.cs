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
using SendGrid.Helpers.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyWebApp1.Services
{
    public class UserService
    {
        private readonly MyDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;


        public UserService(MyDbContext dbContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IEmailService emailService)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
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

                // gán role mặc định user
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

            // lay role user
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
        public async Task<LoginResponseDTO> UpdateProfile(ClaimsPrincipal currentUser, UpdateProfileDTO userDTO)
        {
            try
            {
                // lay UserId từ token
                var currentUserId = int.Parse(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);

                var userToUpdate = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserId == currentUserId);
                if (userToUpdate == null)
                {
                    throw new Exception("User not found.");
                }

                // check xem username mới có bị trùng với user khac khong
                if (!string.IsNullOrEmpty(userDTO.Username) &&
                    _dbContext.Users.Any(u => u.Username == userDTO.Username && u.UserId != currentUserId))
                {
                    throw new Exception("Username already exists.");
                }

                // Cập nhật các fied chỉ khi có giá trị
                if (!string.IsNullOrEmpty(userDTO.Username))
                {
                    userToUpdate.Username = userDTO.Username;
                }
                if (!string.IsNullOrEmpty(userDTO.Fullname))
                {
                    userToUpdate.Fullname = userDTO.Fullname;
                }
                if (!string.IsNullOrEmpty(userDTO.Email))
                {
                    userToUpdate.Email = userDTO.Email;
                }
                if (!string.IsNullOrEmpty(userDTO.PhoneNumber))
                {
                    userToUpdate.PhoneNumber = userDTO.PhoneNumber;
                }
                if (!string.IsNullOrEmpty(userDTO.Address))
                {
                    userToUpdate.Address = userDTO.Address;
                }
                if (!string.IsNullOrEmpty(userDTO.Password))
                {
                    userToUpdate.Password = userDTO.Password;
                }

                // save db
                _dbContext.Users.Update(userToUpdate);
                await _dbContext.SaveChangesAsync();

                // role user
                var userRole = await _dbContext.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userToUpdate.UserId);
                var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.RoleId == userRole.RoleId);

                // tao token mới
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

                // Trả về LoginResponseDTO
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

        public async Task<User> RequestManagerRole()
        {
            // Lấy token từ header Authorization
            var authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                throw new Exception("Authorization token is missing or invalid.");
            }

            // Loại bỏ tiền tố "Bearer " để lấy token
            var token = authHeader.Substring("Bearer ".Length).Trim();

            // Giải mã token để lấy thông tin
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.ReadJwtToken(token);

            // Lấy userId từ claim trong token
            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserId");
            if (userIdClaim == null)
            {
                throw new Exception("UserId not found in token.");
            }

            // Lấy userId
            var userId = int.Parse(userIdClaim.Value);

            // Tìm người dùng từ cơ sở dữ liệu dựa trên userId
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            // Kiểm tra xem người dùng đã yêu cầu vai trò manager chưa
            if (user.IsApprovedUser)
            {
                throw new Exception("User has already requested the manager role.");
            }

            // Đánh dấu người dùng đã yêu cầu vai trò manager
            user.IsApprovedUser = true;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<Models.Pet> AddNewPet(Models.Pet pet, int userId)
        {
            // Gán UserId cho pet
            pet.UserId = userId;

            // Thêm pet vào db
            await _dbContext.Pets.AddAsync(pet);

            // Nếu có ảnh, thêm chúng vào bảng PetImage
            if (pet.PetImages != null && pet.PetImages.Any())
            {
                foreach (var image in pet.PetImages)
                {
                    image.PetId = pet.PetId;
                    await _dbContext.PetImages.AddAsync(image);
                }
            }

            await _dbContext.SaveChangesAsync();

            var userEmail = await _dbContext.Users
                .Where(u => u.UserId == userId)
                .Select(u => u.Email)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(userEmail))
            {
                throw new Exception("User email not found.");
            }

            // Gửi email thông báo
            Mailrequest mailrequest = new Mailrequest
            {
                ToEmail = userEmail,
                Subject = "Pet Request Notification from PawFund",
                Body = "Your request to add a pet on PawFund is awaiting approval. Please wait! Thank you for your contribution."
            };

            await _emailService.SendEmaiAddPetRequest(mailrequest);  // Gọi phương thức gửi email

            return pet;
        }

        public async Task<List<PetListDTO>> GetAllApprovedPets()
        {
            // Lọc các pet có isApproved = true và isAdopted = false
            return await _dbContext.Pets
                .Where(pet => pet.IsApproved && !pet.IsAdopted)
                .Select(pet => new PetListDTO
                {
                    PetId = pet.PetId,
                    PetName = pet.PetName,
                    PetType = pet.PetType,
                    Age = pet.Age,
                    Gender = pet.Gender,
                    Address = pet.Address,
                    MedicalCondition = pet.MedicalCondition,
                    Description = pet.Description,
                    Color = pet.Color,
                    Size = pet.Size,
                    ContactPhoneNumber = pet.ContactPhoneNumber,
                    ContactEmail = pet.ContactEmail,
                    PetCategoryId = pet.PetCategoryId,
                    IsAdopted = pet.IsAdopted,
                    IsApproved = pet.IsApproved,
                    ApprovedByUserId = pet.ApprovedByUserId,
                    ShelterId = pet.ShelterId,
                    ShelterName = _dbContext.Shelters
                        .Where(shelter => shelter.ShelterId == pet.ShelterId)
                        .Select(shelter => shelter.ShelterName)
                        .FirstOrDefault()
                })
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
                Description = pet.Description,
                Size = pet.Size,
                Color = pet.Color,
                ContactPhoneNumber = pet.ContactPhoneNumber,
                ContactEmail = pet.ContactEmail,
                Age = pet.Age,
                Gender = pet.Gender,
                PetType = pet.PetType,
                IsAdopted = pet.IsAdopted,
                IsApproved = pet.IsApproved
            };
        }

        public async Task<List<Pet>> GetPetsByUserId(int userId)
        {
            var pets = await _dbContext.Pets
                .Where(p => p.UserId == userId)
                .Include(p => p.PetImages)
                .ToListAsync();

            if (!pets.Any())
            {
                // Ghi log nếu không tìm thấy pet nào
                Console.WriteLine($"No pets found for user with ID {userId}.");
            }
            else
            {
                Console.WriteLine($"Found {pets.Count} pets for user with ID {userId}.");
            }

            return pets;
        }


        public async Task<List<Pet>> GetPetsByShelter(int shelterId)
        {
            // Lấy danh sách thú cưng đã được duyệt (Approved = true) thuộc shelter
            var pets = await _dbContext.Pets
                                     .Where(p => p.ShelterId == shelterId && p.IsApproved == true)
                                     .ToListAsync();
            return pets;
        }

    }
}
