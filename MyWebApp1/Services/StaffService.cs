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

        private UserDTO MapUserToDTO(User user, Role role)
        {
            return new UserDTO
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                RoleId = role.RoleId,
                ShelterId = (int)user.ShelterId
            };
        }

        public async Task<int?> GetShelterIdByStaffId(int staffId)
        {
            var staff = await _context.Users
                .Where(u => u.UserId == staffId && u.RoleId == 4)
                .FirstOrDefaultAsync();

            if (staff != null)
            {
                return staff.ShelterId;
            }

            return null;
        }

        public async Task<Models.Pet> AddNewPetForStaff(AddNewPetDTO newPetDTO, int userId)
        {
            var shelterId = await _context.Users
                .Where(s => s.UserId == userId)
                .Select(s => s.ShelterId)
                .FirstOrDefaultAsync();

            if (shelterId == null)
            {
                throw new Exception("Staff does not manage any shelter.");
            }

            var pet = new Models.Pet
            {
                PetName = newPetDTO.PetName,
                PetType = newPetDTO.PetType,
                Age = newPetDTO.Age,
                Gender = newPetDTO.Gender,
                Address = newPetDTO.Address,
                MedicalCondition = newPetDTO.MedicalCondition,
                Description = newPetDTO.Description,
                Color = newPetDTO.Color,
                Size = newPetDTO.Size,
                ContactPhoneNumber = newPetDTO.ContactPhoneNumber,
                ContactEmail = newPetDTO.ContactEmail,
                PetCategoryId = newPetDTO.PetCategoryId,
                CreatedAt = DateTime.Now,
                IsAdopted = false,
                IsApproved = true,
                ShelterId = shelterId.Value,
                UserId = userId,
                ApprovedByUserId = userId
            };

            if (newPetDTO.PetImages != null && newPetDTO.PetImages.Any())
            {
                pet.PetImages = newPetDTO.PetImages.Select(imageDto => new PetImage
                {
                    ImageDescription = imageDto.ImageDescription,
                    ImageUrl = imageDto.ImageUrl,
                    IsThumbnailImage = imageDto.IsThumbnailImage
                }).ToList();
            }

            await _context.Pets.AddAsync(pet);
            if (pet.PetImages != null && pet.PetImages.Any())
            {
                foreach (var image in pet.PetImages)
                {
                    image.PetId = pet.PetId;
                    await _context.PetImages.AddAsync(image);
                }
            }

            await _context.SaveChangesAsync();

            var userEmail = await _context.Users
                .Where(u => u.UserId == userId)
                .Select(u => u.Email)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(userEmail))
            {
                throw new Exception("User email not found.");
            }

            // Gửi email thông báo
            var mailrequest = new Mailrequest
            {
                ToEmail = userEmail,
                Subject = "Pet Request Notification from PawFund",
                Body = "Your pet has been added to the shelter you manage successfully. Thank you for your contribution!"
            };

            await _emailService.SendEmail(mailrequest);

            return pet;
        }

        public async Task<bool> UpdatePet(int petId, PetUpdateDTO updatedPet)
        {
            var pet = await _context.Pets.FirstOrDefaultAsync(p => p.PetId == petId);

            if (pet == null)
            {
                throw new Exception("Pet not found.");
            }

            // chi update nhung thong tin duoc nhap
            if (!string.IsNullOrEmpty(updatedPet.PetName))
            {
                pet.PetName = updatedPet.PetName;
            }

            if (!string.IsNullOrEmpty(updatedPet.PetType))
            {
                pet.PetType = updatedPet.PetType;
            }

            if (updatedPet.Age.Equals(0))
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

            await _context.SaveChangesAsync();

            return true;
        }


        // lay list pet staff qli
        public async Task<List<Pet>> GetPetsByShelterForStaff(int userId)
        {
            var user = await _context.Users
                                     .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            // Lấy roleId để xác nhận người dùng là staff
            var roleId = await _context.UserRoles
                                       .Where(ur => ur.UserId == user.UserId)
                                       .Select(ur => ur.RoleId)
                                       .FirstOrDefaultAsync();

            if (roleId != 4)
            {
                throw new Exception("User is not a staff.");
            }

            // Lấy danh sách các pet mà staff quản lý, bao gồm ImageUrl của các PetImage
            var pets = await _context.Pets
                                     .Where(p => p.ShelterId == user.ShelterId)
                                     .Select(p => new Pet
                                     {
                                         PetId = p.PetId,
                                         PetName = p.PetName,
                                         PetType = p.PetType,
                                         Age = p.Age,
                                         Gender = p.Gender,
                                         Address = p.Address,
                                         MedicalCondition = p.MedicalCondition,
                                         Description = p.Description,
                                         Color = p.Color,
                                         Size = p.Size,
                                         ContactPhoneNumber = p.ContactPhoneNumber,
                                         ContactEmail = p.ContactEmail,
                                         CreatedAt = p.CreatedAt,
                                         IsAdopted = p.IsAdopted,
                                         IsApproved = p.IsApproved,
                                         ShelterId = p.ShelterId,
                                         PetCategoryId = p.PetCategoryId,                      
                                         PetImages = p.PetImages.Select(pi => new PetImage
                                         {
                                             PetImageId = pi.PetImageId,
                                             ImageUrl = pi.ImageUrl,
                                             ImageDescription = pi.ImageDescription,
                                             IsThumbnailImage = pi.IsThumbnailImage
                                         }).ToList()
                                     })
                                     .ToListAsync();

            return pets;
        }

        public async Task<bool> ApproveAdoptionByStaff(int userId, int adoptionId, ApproveAdoptionRequestDto request)
        {
            var user = await _context.Users
                                     .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var roleId = await _context.UserRoles
                                       .Where(ur => ur.UserId == user.UserId)
                                       .Select(ur => ur.RoleId)
                                       .FirstOrDefaultAsync();

            if (roleId != 4 && roleId != 3)
            {
                throw new Exception("You do not have permission.");
            }

            var adoption = await _context.Adoptions.FindAsync(adoptionId);
            var pet = await _context.Pets.FindAsync(adoption.PetId);

            if (pet.ShelterId != user.ShelterId)
            {
                throw new Exception("You can only approve adoptions for pets in your shelter.");
            }
            // Cập nhật trạng thái phê duyệt cho yêu cầu nhận nuôi
            adoption.IsApproved = request.IsApproved;

            //từ chối
            if (request.IsApproved != 1)
            {
                adoption.Reason = request.Reason;
            }
            else
            {
                //phê duyệt
                pet.IsAdopted = true;
            }

            await _context.SaveChangesAsync();

            var userEmail = adoption.Email;
            if (string.IsNullOrEmpty(userEmail))
            {
                throw new Exception("Email not found.");
            }

            // Gửi email thông báo
            Mailrequest mailrequest = new Mailrequest
            {
                ToEmail = userEmail,
                Subject = "Pet Adoption Notification from PawFund",
                Body = "Your adoption status has been changed. Please, go to PawFund for details!"
            };

            await _emailService.SendEmail(mailrequest);

            return true;
        }


        public IEnumerable<object> GetAdoptions()
        {
            // adoption chua duyet trong shelter
            var adoptions = from a in _context.Adoptions
                            join u in _context.Users on a.UserId equals u.UserId
                            join p in _context.Pets on a.PetId equals p.PetId
                            join s in _context.Shelters on p.ShelterId equals s.ShelterId 
                            //where a.IsApproved == 2 && a.IsApproved == 0 && a.IsApproved == 1
                            select new
                            {
                                a.AdoptionId,
                                a.UserId,
                                a.PetId,
                                a.IsApproved,
                                a.Note,
                                Username = u.Username,
                                PetName = p.PetName,
                                ShelterName = s.ShelterName,
                                a.SelfDescription,
                                a.HasPetExperience,
                                a.ReasonForAdopting,
                                a.Reason
                            };

            return adoptions.ToList();
        }

        public IEnumerable<object> GetAdoptionsByShelterId(int shelterId)
        {
            var adoptions = from a in _context.Adoptions
                            join u in _context.Users on a.UserId equals u.UserId
                            join p in _context.Pets on a.PetId equals p.PetId
                            join s in _context.Shelters on p.ShelterId equals s.ShelterId
                            where p.ShelterId == shelterId
                            select new
                            {
                                a.AdoptionId,
                                a.UserId,
                                a.PetId,
                                a.IsApproved,
                                a.Note,
                                Username = u.Username,
                                PetName = p.PetName,
                                ShelterName = s.ShelterName,
                                a.SelfDescription,
                                a.HasPetExperience,
                                a.ReasonForAdopting,
                                a.createDate,
                                a.Reason

                            };

            return adoptions.ToList();
        }


    }
}
