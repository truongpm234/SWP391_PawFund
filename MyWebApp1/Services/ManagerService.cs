using Microsoft.EntityFrameworkCore;
using MyWebApp1.Data;
using MyWebApp1.DTO;
using MyWebApp1.Models;
using System.Threading.Tasks;

namespace MyWebApp1.Services
{
    public class ManagerService
    {
        private readonly MyDbContext _context;
        private readonly IEmailService _emailService;

        public ManagerService(MyDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        //public async Task<Models.Pet> ApprovePet(int petId, int approvedByUserId, int shelterId)
        //{
        //    // Tìm thú cưng trong cơ sở dữ liệu
        //    var existingPet = await _context.Pets.FindAsync(petId);
        //    if (existingPet == null)
        //    {
        //        throw new Exception("Pet not found");
        //    }

        //    // Tìm thông tin shelter dựa trên shelterId
        //    var shelter = await _context.Shelters.FindAsync(shelterId);
        //    if (shelter == null)
        //    {
        //        throw new Exception("Shelter not found");
        //    }

        //    existingPet.IsApproved = true;
        //    existingPet.ShelterId = shelterId;
        //    existingPet.ApprovedByUserId = approvedByUserId;

        //    // Cập nhật trạng thái của thú cưng
        //    _context.Entry(existingPet).State = EntityState.Modified;

        //    // Lưu thay đổi vào cơ sở dữ liệu
        //    await _context.SaveChangesAsync();

        //    return existingPet;
        //}

        public async Task<Models.Pet> ApprovePet(int petId, int approvedByUserId, int shelterId)
        {
            // Tìm thú cưng trong cơ sở dữ liệu
            var existingPet = await _context.Pets.FindAsync(petId);
            if (existingPet == null)
            {
                throw new Exception("Pet not found");
            }

            // Tìm thông tin shelter dựa trên shelterId
            var shelter = await _context.Shelters.FindAsync(shelterId);
            if (shelter == null)
            {
                throw new Exception("Shelter not found");
            }

            // Duyệt thú cưng
            existingPet.IsApproved = true;
            existingPet.ShelterId = shelterId;
            existingPet.ApprovedByUserId = approvedByUserId;

            // Cập nhật trạng thái của thú cưng
            _context.Entry(existingPet).State = EntityState.Modified;

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            // gửi thông báo
            var userEmail = existingPet.ContactEmail;
            if (string.IsNullOrEmpty(userEmail))
            {
                throw new Exception("User email not found.");
            }

            // gửi email thông báo
            Mailrequest mailrequest = new Mailrequest
            {
                ToEmail = userEmail,
                Subject = "Pet Approval Notification from PawFund",
                Body = $"Your pet {existingPet.PetName} has been approved!"
            };

            await _emailService.SendEmailAddNewPetAsync(mailrequest);

            return existingPet;
        }
        public async Task DeletePet(int petId)
        {
            var pet = await _context.Pets.FindAsync(petId);
            if (pet == null)
            {
                throw new Exception("Pet not found");
            }

            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Pet>> GetAllPets()
        {
            return await _context.Pets.ToListAsync();
        }
    }
}
