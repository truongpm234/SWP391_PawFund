using MyWebApp1.Data;
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

        public bool CreateAdoptionRequest(AdoptionRequestModel request)
        {
            var adoption = new Adoption
            {
                UserId = request.UserId,
                PetId = request.PetId,
                IsApproved = false,
                Note = request.Note
            };

            _context.Adoptions.Add(adoption);
            return _context.SaveChanges() > 0;
        }

        public IEnumerable<object> GetAdoptions()
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
                                PetName = p.PetName
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

    }
}
