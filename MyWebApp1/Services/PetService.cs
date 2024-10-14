using Microsoft.EntityFrameworkCore;
using MyWebApp1.Data;
using MyWebApp1.Models;
using MyWebApp1.Payload.Request;
using MyWebApp1.Payload.Response;

namespace MyWebApp1.Services
{
    public class PetService
    {
        private readonly MyDbContext _context;

        public PetService(MyDbContext context)
        {
            _context = context;
        }

        public async Task<Models.Pet> AddNewPet(Models.Pet pet)
        {
            // Tạo đối tượng Pet mới từ thông tin đầu vào
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

            // Thêm vào DB
            await _context.Pets.AddAsync(Addpet);
            await _context.SaveChangesAsync();

            // Trả về đối tượng
            return Addpet;
        }

        public async Task<List<PetResponse>> GetAllPets()
        {
            var pets = await _context.Pets.ToListAsync();
            var petList = new List<PetResponse>();
            foreach (var item in pets)
            {
                var petResponse = new PetResponse()
                {
                    PetId = item.PetId,
                    PetName = item.PetName,
                    Address = item.Address,
                    MedicalCondition = item.MedicalCondition,
                    ContactPhoneNumber = item.ContactPhoneNumber,
                    ContactEmail = item.ContactEmail,
                    Age = item.Age,
                    Gender = item.Gender,
                    PetType = item.PetType,
                    IsAdopted = item.IsAdopted,
                    IsApproved = item.IsApproved
                };
                petList.Add(petResponse);
            }
            return petList;
        }

        public async Task<PetResponse> GetPet(int petId)
        {
            var pet = await _context.Pets
                .FirstOrDefaultAsync(pet => pet.PetId == petId);
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

        public async Task DeletePet(int id)
        {
            _context.Pets.Remove(new Pet()
            {
                PetId = id
            });
            await _context.SaveChangesAsync();
        }

        public async Task<Models.Pet> UpdatePet(Models.Pet pet)
        {
            // Kiểm tra xem PetId có hợp lệ không
            if (pet.PetId <= 0)
            {
                throw new Exception("PetId is required");
            }

            // Tìm pet hiện tại trong cơ sở dữ liệu
            var existingPet = await _context.Pets.FindAsync(pet.PetId);
            if (existingPet == null)
            {
                throw new Exception("Pet không tồn tại");
            }

            // Cập nhật thông tin của pet
            existingPet.PetName = pet.PetName;
            existingPet.PetType = pet.PetType;
            existingPet.Age = pet.Age;
            existingPet.Gender = pet.Gender;
            existingPet.Address = pet.Address;
            existingPet.MedicalCondition = pet.MedicalCondition;
            existingPet.ContactPhoneNumber = pet.ContactPhoneNumber;
            existingPet.ContactEmail = pet.ContactEmail;
            existingPet.PetCategoryId = pet.PetCategoryId; // Giả định rằng PetCategoryId đã được kiểm tra ở nơi khác
            existingPet.IsAdopted = pet.IsAdopted;
            existingPet.IsApproved = pet.IsApproved;

            // Cập nhật trạng thái
            _context.Entry(existingPet).State = EntityState.Modified;

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            // Trả về đối tượng đã được cập nhật
            return existingPet;
        }

    }
}
