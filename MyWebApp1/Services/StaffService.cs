using Microsoft.EntityFrameworkCore;
using MyWebApp1.Data;
using MyWebApp1.Models;
using MyWebApp1.Models.MyWebApp1.Models;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using MyWebApp1.DTO;


namespace MyWebApp1.Services
{
    public class StaffService
    {
        private readonly MyDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;

        public StaffService(MyDbContext context, IHttpContextAccessor httpContextAccessor, IEmailService emailService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;

        }

        // Hàm ánh xạ từ User sang UserDTO
        private UserDTO MapUserToDTO(User user, Role role)
        {
            return new UserDTO
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                RoleId = role.RoleId, // Lấy RoleId từ bảng Role
                ShelterId = (int)user.ShelterId // Bổ sung thêm các thuộc tính khác nếu cần
            };
        }

        public async Task<bool> UpdatePet(int petId, PetUpdateDTO updatedPet)
        {
            // Tìm thú cưng theo PetId
            var pet = await _context.Pets.FirstOrDefaultAsync(p => p.PetId == petId);

            if (pet == null)
            {
                throw new Exception("Pet not found.");
            }

            // Cập nhật chỉ những trường được nhập trong yêu cầu
            if (!string.IsNullOrEmpty(updatedPet.PetName))
            {
                pet.PetName = updatedPet.PetName;
            }

            if (!string.IsNullOrEmpty(updatedPet.PetType))
            {
                pet.PetType = updatedPet.PetType;
            }

            if (updatedPet.Age > 0) // Kiểm tra xem độ tuổi có lớn hơn 0 không
            {
                pet.Age = updatedPet.Age;
            }

            if (!string.IsNullOrEmpty(updatedPet.Gender))
            {
                pet.Gender = updatedPet.Gender;
            }

            if (!string.IsNullOrEmpty(updatedPet.Address))
            {
                pet.Address = updatedPet.Address;
            }

            if (!string.IsNullOrEmpty(updatedPet.MedicalCondition))
            {
                pet.MedicalCondition = updatedPet.MedicalCondition;
            }
            if (!string.IsNullOrEmpty(updatedPet.Description))
            {
                pet.MedicalCondition = updatedPet.Description;
            }
            if (!string.IsNullOrEmpty(updatedPet.Color))
            {
                pet.MedicalCondition = updatedPet.Color;
            }
            if (!string.IsNullOrEmpty(updatedPet.Size))
            {
                pet.MedicalCondition = updatedPet.Size;
            }

            if (updatedPet.PetCategoryId > 0)
            {
                pet.PetCategoryId = updatedPet.PetCategoryId;
            }

            // Lưu thay đổi vào database
            await _context.SaveChangesAsync();

            return true;
        }


        // Lấy danh sách thú cưng thuộc Shelter của Staff
        public async Task<List<Pet>> GetPetsByShelterForStaff(int userId)
        {
            var user = await _context.Users
                                     .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            // Truy vấn RoleId từ bảng UserRoles
            var roleId = await _context.UserRoles
                                       .Where(ur => ur.UserId == user.UserId)
                                       .Select(ur => ur.RoleId)
                                       .FirstOrDefaultAsync();

            // Kiểm tra RoleId, chỉ cho phép Staff (RoleId = 4)
            if (roleId != 4)
            {
                throw new Exception("User is not a staff.");
            }

            // Lấy danh sách thú cưng thuộc shelter mà staff đang quản lý
            var pets = await _context.Pets
                                     .Where(p => p.ShelterId == user.ShelterId)
                                     .ToListAsync();

            return pets;
        }

        //public async Task<bool> ApproveAdoptionByStaff(int userId, int adoptionId)
        //{
        //    // Truy vấn user từ bảng Users
        //    var user = await _context.Users
        //                             .FirstOrDefaultAsync(u => u.UserId == userId);

        //    if (user == null)
        //    {
        //        throw new Exception("User not found.");
        //    }

        //    // Truy vấn RoleId từ bảng UserRoles (nếu bạn có bảng này)
        //    var roleId = await _context.UserRoles
        //                               .Where(ur => ur.UserId == user.UserId)
        //                               .Select(ur => ur.RoleId)
        //                               .FirstOrDefaultAsync();

        //    // Kiểm tra RoleId
        //    if (roleId != 4)
        //    {
        //        throw new Exception("User is not a staff.");
        //    }

        //    // Tìm adoption và pet tương ứng
        //    var adoption = await _context.Adoptions.FindAsync(adoptionId);
        //    var pet = await _context.Pets.FindAsync(adoption.PetId);

        //    // Kiểm tra xem thú cưng có thuộc Shelter mà Staff quản lý không
        //    if (pet.ShelterId != user.ShelterId)
        //    {
        //        throw new Exception("You can only approve adoptions for pets in your shelter.");
        //    }

        //    // Nếu hợp lệ, duyệt yêu cầu adoption
        //    adoption.IsApproved = true;

        //    await _context.SaveChangesAsync();

        //    return true;
        //}
        public async Task<bool> ApproveAdoptionByStaff(int userId, int adoptionId, ApproveAdoptionRequestDto request)
        {
            // Truy vấn user từ bảng Users
            var user = await _context.Users
                                     .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            // Truy vấn RoleId từ bảng UserRoles
            var roleId = await _context.UserRoles
                                       .Where(ur => ur.UserId == user.UserId)
                                       .Select(ur => ur.RoleId)
                                       .FirstOrDefaultAsync();

            // Kiểm tra RoleId
            if (roleId != 4)
            {
                throw new Exception("User is not a staff.");
            }

            // Tìm adoption và pet tương ứng
            var adoption = await _context.Adoptions.FindAsync(adoptionId);
            var pet = await _context.Pets.FindAsync(adoption.PetId);

            // Kiểm tra xem thú cưng có thuộc Shelter mà Staff quản lý không
            if (pet.ShelterId != user.ShelterId)
            {
                throw new Exception("You can only approve adoptions for pets in your shelter.");
            }

            // Nếu hợp lệ, duyệt yêu cầu adoption
            adoption.IsApproved = request.IsApproved;

            // Nếu yêu cầu từ chối, thêm lý do vào trường reason
            if (!request.IsApproved)
            {
                adoption.Reason = request.Reason; // Cập nhật vào trường reason
            }

            await _context.SaveChangesAsync();
            // gửi thông báo
            var userEmail = adoption.Email;
            if (string.IsNullOrEmpty(userEmail))
            {
                throw new Exception("Email not found.");
            }

            // gửi email thông báo
            Mailrequest mailrequest = new Mailrequest
            {
                ToEmail = userEmail,
                Subject = "Pet Adoption Notification from PawFund",
                Body = "Your adoption staus pet has been changed. Please, go to PawFund for detail!"
            };

            await _emailService.SendEmailAdoptionAsync(mailrequest);

            return true;
        }


        public IEnumerable<object> GetAdoptions()
        {
            // Lọc các yêu cầu adoption chưa được phê duyệt và lấy thêm thông tin Shelter
            var adoptions = from a in _context.Adoptions
                            join u in _context.Users on a.UserId equals u.UserId
                            join p in _context.Pets on a.PetId equals p.PetId
                            join s in _context.Shelters on p.ShelterId equals s.ShelterId // Kết nối với Shelter để lấy thông tin shelter
                            where a.IsApproved == false // Chỉ lấy các yêu cầu chưa được phê duyệt
                            select new
                            {
                                a.AdoptionId,
                                a.UserId,
                                a.PetId,
                                a.IsApproved,
                                a.Note,
                                Username = u.Username,
                                PetName = p.PetName,
                                ShelterName = s.ShelterName, // Hiển thị tên shelter mà pet đang ở
                                a.SelfDescription,
                                a.HasPetExperience,
                                a.ReasonForAdopting
                            };

            return adoptions.ToList();
        }

    }
}
