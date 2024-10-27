using MyWebApp1.Data;
using MyWebApp1.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyWebApp1.Services
{
    public interface IShelterService
    {
        Shelter GetShelterById(int id);
        IEnumerable<Shelter> GetAllShelters();
    }


}
