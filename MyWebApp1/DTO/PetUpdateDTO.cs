﻿namespace MyWebApp1.DTO
{
    public class PetUpdateDTO
    {
        public string? PetName { get; set; }
        public string? PetType { get; set; }
        public string Age { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? MedicalCondition { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string? ContactPhoneNumber { get; set; }
        public string? ContactEmail { get; set; }
        public int? PetCategoryId { get; set; }
    }
}
