using Microsoft.EntityFrameworkCore;
using MyWebApp1.Data;
using MyWebApp1.DTO;
using MyWebApp1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyWebApp1.Services
{
    public class ShelterService : IShelterService
    {
        private readonly MyDbContext _context;
        private readonly UserService _userService;

        public ShelterService(MyDbContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<IEnumerable<ShelterWithPetsDTO>> GetAllSheltersWithPetsAsync()
        {
            var shelters = await _context.Shelters.ToListAsync();
            var shelterWithPetsList = new List<ShelterWithPetsDTO>();

            foreach (var shelter in shelters)
            {
                var approvedPets = await _userService.GetPetsByShelter(shelter.ShelterId);

                shelterWithPetsList.Add(new ShelterWithPetsDTO
                {
                    ShelterId = shelter.ShelterId,
                    ShelterName = shelter.ShelterName,
                    ShelterLocation = shelter.ShelterLocation,
                    ApprovedPets = approvedPets,
                    Capacity = shelter.Capacity,
                    Contact = shelter.Contact,
                    Email = shelter.Email,
                    OpeningClosing = shelter.OpeningClosing,
                    ShelterImage = shelter.ShelterImage,
                    Description = shelter.Description
                });
            }

            return shelterWithPetsList;
        }

        public async Task<ShelterWithPetsDTO> GetShelterWithPetsByIdAsync(int shelterId)
        {
            var shelter = await _context.Shelters.FindAsync(shelterId);
            if (shelter == null) return null;

            var approvedPets = await _userService.GetPetsByShelter(shelterId);

            return new ShelterWithPetsDTO
            {
                ShelterId = shelter.ShelterId,
                ShelterName = shelter.ShelterName,
                ShelterLocation = shelter.ShelterLocation,
                ApprovedPets = approvedPets,
                Capacity = shelter.Capacity,
                Contact = shelter.Contact,
                Email = shelter.Email,
                OpeningClosing = shelter.OpeningClosing,
                ShelterImage = shelter.ShelterImage,
                Description = shelter.Description
            };
        }

        public async Task<Shelter> CreateShelterAsync(ShelterCreateDTO shelterDto)
        {
            var shelter = new Shelter
            {
                //ShelterId = shelterDto.ShelterId,
                ShelterLocation = shelterDto.ShelterLocation,
                ShelterName = shelterDto.ShelterName,
                Capacity = (int)shelterDto.Capacity,
                Contact = shelterDto.Contact,
                Email = shelterDto.Email,
                OpeningClosing = shelterDto.OpeningClosing,
                ShelterImage = shelterDto.ShelterImage,
                Description = shelterDto.Description
            };

            _context.Shelters.Add(shelter);
            await _context.SaveChangesAsync();

            return shelter;
        }

        public async Task<bool> UpdateShelterAsync(int shelterId, ShelterCreateDTO shelterDto)
        {
            var shelter = await _context.Shelters.FindAsync(shelterId);
            if (shelter == null)
            {
                return false;
            }

            // Chỉ cập nhật những thuộc tính nào được truyền vào
            if (!string.IsNullOrEmpty(shelterDto.ShelterName))
            {
                shelter.ShelterName = shelterDto.ShelterName;
            }

            if (!string.IsNullOrEmpty(shelterDto.ShelterLocation))
            {
                shelter.ShelterLocation = shelterDto.ShelterLocation;
            }

            if (shelterDto.Capacity > 0)
            {
                shelter.Capacity = (int)shelterDto.Capacity;
            }

            if (!string.IsNullOrEmpty(shelterDto.Contact))
            {
                shelter.Contact = shelterDto.Contact;
            }

            if (!string.IsNullOrEmpty(shelterDto.Email))
            {
                shelter.Email = shelterDto.Email;
            }

            if (!string.IsNullOrEmpty(shelterDto.OpeningClosing))
            {
                shelter.OpeningClosing = shelterDto.OpeningClosing;
            }

            if (!string.IsNullOrEmpty(shelterDto.ShelterImage))
            {
                shelter.ShelterImage = shelterDto.ShelterImage;
            }

            if (!string.IsNullOrEmpty(shelterDto.Description))
            {
                shelter.Description = shelterDto.Description;
            }
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteShelterAsync(int shelterId)
        {
            var shelter = await _context.Shelters.FindAsync(shelterId);
            if (shelter == null)
            {
                return false;
            }

            _context.Shelters.Remove(shelter);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
