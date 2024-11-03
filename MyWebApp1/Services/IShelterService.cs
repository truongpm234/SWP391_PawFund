using MyWebApp1.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyWebApp1.Services
{
    public interface IShelterService
    {
        Task<IEnumerable<ShelterWithPetsDTO>> GetAllSheltersWithPetsAsync();
        Task<ShelterWithPetsDTO> GetShelterWithPetsByIdAsync(int shelterId);
    }
}
