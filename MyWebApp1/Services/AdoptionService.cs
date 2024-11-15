using Microsoft.EntityFrameworkCore;
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
        private readonly IEmailService _emailService;
        public AdoptionService(MyDbContext context)
        {
            _context = context;
        }

        public bool CreateAdoptionRequest(AdoptionRequestModel request, int userId, int petId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                throw new Exception("User not found.");
            }
            //if (!user.IsApprovedUser)
            //{
            //    throw new Exception("User is not approved for adoption requests. Please complete the verification process.");
            //}

            // Kiểm tra thông tin thú cưng
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

            // Tạo yêu cầu nhận nuôi
            var adoption = new Adoption
            {
                UserId = userId,
                PetId = petId,
                FullName = request.FullName,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                SelfDescription = request.SelfDescription,
                HasPetExperience = request.HasPetExperience,
                ReasonForAdopting = request.ReasonForAdopting,
                IsApproved = false,
                Note = request.Note,
                createDate = request.createDate,
            };

            _context.Adoptions.Add(adoption);

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
            var adoption = _context.Adoptions.FirstOrDefault(a => a.AdoptionId == adoptionId);
            if (adoption == null)
            {
                return false;
            }

            adoption.IsApproved = true;

            var pet = _context.Pets.FirstOrDefault(p => p.PetId == adoption.PetId);
            if (pet != null)
            {
                pet.IsAdopted = true;
            }
            else
            {
                throw new Exception("Pet not found.");
            }

            return _context.SaveChanges() > 0;
        }

        public IEnumerable<object> GetAdoptionsByUser(int userId)
        {
            var userAdoptions = from a in _context.Adoptions
                                join p in _context.Pets on a.PetId equals p.PetId
                                where a.UserId == userId  
                                select new
                                {
                                    a.AdoptionId,
                                    a.PetId,
                                    a.IsApproved,
                                    a.Note,
                                    PetName = p.PetName,
                                    a.SelfDescription,
                                    a.HasPetExperience,
                                    a.ReasonForAdopting,
                                    a.Reason
                                };

            return userAdoptions.ToList();
        }

        public async Task CheckAndSendReminderAsync()
        {
            var adoptionConfirmations = await _context.AdoptionConfirmations
                .Where(ac => ac.ConfirmationDate.HasValue && !ac.ReminderSent)
                .ToListAsync();

            foreach (var confirmation in adoptionConfirmations)
            {
                if (confirmation.ConfirmationDate.Value.AddDays(30) <= DateTime.UtcNow)
                {
                    // Lấy thông tin nhận nuôi từ Adoption
                    var adoption = await _context.Adoptions.FindAsync(confirmation.AdoptionId);
                    var userEmail = adoption?.Email;

                    if (!string.IsNullOrEmpty(userEmail))
                    {
                        var mailRequest = new Mailrequest
                        {
                            ToEmail = userEmail,
                            Subject = "Reminder: Adoption Confirmation",
                            Body = "It's been 30 days since your adoption approval. Please confirm if you still have the pet."
                        };

                        try
                        {
                            await _emailService.SendEmail(mailRequest);

                            confirmation.ReminderSent = true;
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Email sending failed: {ex.Message}");
                        }
                    }
                }
            }
        }
    }
}
