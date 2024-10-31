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
                IsApproved = false,  
                Note = request.Note,
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

    }
}
