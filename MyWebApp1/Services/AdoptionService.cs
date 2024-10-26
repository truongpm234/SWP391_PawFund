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
                IsApproved = false,  // Initially, it’s not approved
                Note = request.Note
            };

            _context.Adoptions.Add(adoption);
            return _context.SaveChanges() > 0;
        }

        public IEnumerable<Adoption> GetAdoptions()
        {
            return _context.Adoptions.ToList();
        }

        public bool ApproveAdoption(int adoptionId)
        {
            var adoption = _context.Adoptions.FirstOrDefault(a => a.AdoptionId == adoptionId);
            if (adoption == null)
            {
                return false;
            }

            adoption.IsApproved = true;
            return _context.SaveChanges() > 0;
        }
    }
}
