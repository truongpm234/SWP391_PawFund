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

        public async Task<List<UserApprove>> GetApprovedRequestsAsync()
        {
            return await _context.UserApproves
                .Where(x => x.IsApprovedUser == true)
                .ToListAsync();
        }

        public async Task<List<UserApprove>> GetPendingRequestsAsync()
        {
            return await _context.UserApproves
                .Where(x => x.IsApprovedUser == false)
                .ToListAsync();
        }

        

        //public async Task<Models.Pet> ApprovePet(int petId, int approvedByUserId, int shelterId)
        //{
        //    var existingPet = await _context.Pets.FindAsync(petId);
        //    if (existingPet == null)
        //    {
        //        throw new Exception("Pet not found");
        //    }

        //    var shelter = await _context.Shelters.FindAsync(shelterId);
        //    if (shelter == null)
        //    {
        //        throw new Exception("Shelter not found");
        //    }

        //    existingPet.IsApproved = true;
        //    existingPet.ShelterId = shelterId;
        //    existingPet.ApprovedByUserId = approvedByUserId;

        //    _context.Entry(existingPet).State = EntityState.Modified;

        //    await _context.SaveChangesAsync();

        //    var userEmail = existingPet.ContactEmail;
        //    if (string.IsNullOrEmpty(userEmail))
        //    {
        //        throw new Exception("User email not found.");
        //    }

        //    Mailrequest mailrequest = new Mailrequest
        //    {
        //        ToEmail = userEmail,
        //        Subject = "Pet Approval Notification from PawFund",
        //        Body = $"Your pet {existingPet.PetName} has been approved!"
        //    };

        //    await _emailService.SendEmail(mailrequest);

        //    return existingPet;
        //}

        public async Task DeletePet(int petId)
        {
            var pet = await _context.Pets
                .Include(p => p.PetImages)
                .Include(p => p.User) 
                .FirstOrDefaultAsync(p => p.PetId == petId);

            if (pet == null)
            {
                throw new Exception("Pet not found");
            }

            _context.PetImages.RemoveRange(pet.PetImages);

            // Sau đó xóa Pet
            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();

            var userEmail = pet.User?.Email;
            if (string.IsNullOrEmpty(userEmail))
            {
                throw new Exception("User email not found.");
            }

            Mailrequest mailrequest = new Mailrequest
            {
                ToEmail = userEmail,
                Subject = "Notification about your pet on PawFund!",
                Body = "Your Pet on PawFund has been deleted by a manager due to invalid information. For more details, contact us at PawFundPet@gmail.com."
            };

            await _emailService.SendEmail(mailrequest);
        }

        public async Task<List<Pet>> GetAllPets()
        {
            return await _context.Pets
                .Include(p => p.PetImages) 
                .ToListAsync();
        }

    }
}
