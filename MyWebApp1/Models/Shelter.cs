﻿using MyWebApp1.Models.MyWebApp1.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApp1.Models
{
    [Table(name: "Shelter")]
    public class Shelter
    {
        [Key]
        public int ShelterId { get; set; }
        public string ShelterLocation { get; set; }
        public string ShelterName { get; set; }
        public int Capacity { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string OpeningClosing { get; set; }
        public string ShelterImage { get; set; }
        public string Description { get; set; }
        [NotMapped]
        public User? User { get; set; }
    }
}