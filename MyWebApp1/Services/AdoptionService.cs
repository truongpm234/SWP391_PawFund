using MyWebApp1.Data;
using MyWebApp1.DTO;
using MyWebApp1.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyWebApp1.Services
{
    public class AdoptionService
    {
        private readonly MyDbContext _context;

        public AdoptionService(MyDbContext context)
        {
            _context = context;
        }

        public bool CreateAdoptionRequest(AdoptionRequestModel request, int userId, int petId)
        {
            // Kiểm tra xem Pet có tồn tại và được phê duyệt không
            var pet = _context.Pets.FirstOrDefault(p => p.PetId == petId);

            if (pet == null)
            {
                throw new Exception("Pet not found.");
            }

            if (pet.IsAdopted)
            {
                throw new Exception("This pet has already been adopted.");
            }

            if (!pet.IsApproved)
            {
                throw new Exception("This pet is not approved for adoption.");
            }

            // Tạo yêu cầu nhận nuôi mới với thông tin từ request
            var adoption = new Adoption
            {
                UserId = userId,
                PetId = petId, // Sử dụng petId từ tham số
                FullName = request.FullName,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                SelfDescription = request.SelfDescription,
                HasPetExperience = request.HasPetExperience,
                ReasonForAdopting = request.ReasonForAdopting,
                IsApproved = false,  // Yêu cầu nhận nuôi chưa được phê duyệt
                Note = request.Note,
            };

            // Thêm yêu cầu nhận nuôi vào cơ sở dữ liệu
            _context.Adoptions.Add(adoption);

            // Lưu yêu cầu vào cơ sở dữ liệu
            return _context.SaveChanges() > 0;
        }

        public IEnumerable<object> GetAdoptions(int userId)
        {

            var adoptions = from a in _context.Adoptions
                            join u in _context.Users on a.UserId equals u.UserId
                            join p in _context.Pets on a.PetId equals p.PetId
                            select new
                            {
                                a.AdoptionId,
                                a.UserId,
                                a.PetId,
                                a.IsApproved,
                                a.Note,
                                Username = u.Username,
                                PetName = p.PetName,
                                a.SelfDescription,
                                a.HasPetExperience,
                                a.ReasonForAdopting
                            };

            return adoptions.ToList();
        }

        public bool ApproveAdoption(int adoptionId)
        {
            // Find the adoption request by its ID
            var adoption = _context.Adoptions.FirstOrDefault(a => a.AdoptionId == adoptionId);
            if (adoption == null)
            {
                return false; // Return false if the adoption request is not found
            }

            // Set the adoption request as approved
            adoption.IsApproved = true;

            // Find the corresponding pet by PetId and update its isAdopted status to true
            var pet = _context.Pets.FirstOrDefault(p => p.PetId == adoption.PetId);
            if (pet != null)
            {
                pet.IsAdopted = true; // Mark the pet as adopted
            }
            else
            {
                throw new Exception("Pet not found.");
            }

            // Save the changes in the database
            return _context.SaveChanges() > 0;
        }

        public IEnumerable<object> GetAdoptionsByUser(int userId)
        {
            // Lấy tất cả các yêu cầu nhận nuôi của người dùng dựa trên UserId
            var userAdoptions = from a in _context.Adoptions
                                join p in _context.Pets on a.PetId equals p.PetId
                                where a.UserId == userId  // Điều kiện để chỉ lấy yêu cầu của user đó
                                select new
                                {
                                    a.AdoptionId,
                                    a.PetId,
                                    a.IsApproved,  // Bao gồm trạng thái đã được phê duyệt
                                    a.Note,
                                    PetName = p.PetName,
                                    a.SelfDescription,
                                    a.HasPetExperience,
                                    a.ReasonForAdopting,
                                    a.Reason
                                };

            return userAdoptions.ToList();
        }

    }
}
