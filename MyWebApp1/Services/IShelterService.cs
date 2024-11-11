using MyWebApp1.DTO;
using MyWebApp1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyWebApp1.Services
{
    public interface IShelterService
    {
        Task<IEnumerable<ShelterWithPetsDTO>> GetAllSheltersWithPetsAsync();
        Task<ShelterWithPetsDTO> GetShelterWithPetsByIdAsync(int shelterId);
        Task<Shelter> CreateShelterAsync(ShelterCreateDTO shelterDto);
        Task<bool> UpdateShelterAsync(int shelterId, ShelterCreateDTO shelterDto);
        Task<bool> DeleteShelterAsync(int shelterId);
    }
}