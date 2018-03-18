using System;
using System.ComponentModel.DataAnnotations;

namespace mvc_auth.Models
{
    public class Organization
    {
        public int ID { get; set; }
        [Required]
        public ApplicationUser User { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}