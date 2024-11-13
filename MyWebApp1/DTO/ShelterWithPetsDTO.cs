﻿using MyWebApp1.Models;
using System.Net.Http;

namespace MyWebApp1.DTO
{
    public class ShelterWithPetsDTO
    {
        public int ShelterId { get; set; }
        public string ShelterName { get; set; }
        public string ShelterLocation { get; set; }
        public int Capacity { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string OpeningClosing { get; set; }
        public string ShelterImage { get; set; }
        public string Description { get; set; }
        public List<Pet> ApprovedPets { get; set; }
    }

}
