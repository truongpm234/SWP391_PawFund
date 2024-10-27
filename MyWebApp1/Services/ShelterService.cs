using MyWebApp1.Data;
using MyWebApp1.Models;
using MyWebApp1.Services;

namespace MyWebApp1.Services
{
    public class ShelterService : IShelterService
    {
        private readonly MyDbContext _context;

        public ShelterService(MyDbContext context)
        {
            _context = context;
        }

        public Shelter GetShelterById(int id)
        {
            return _context.Shelters.Find(id);
        }

        public IEnumerable<Shelter> GetAllShelters()
        {
            return _context.Shelters.ToList();
        }
    }
}
