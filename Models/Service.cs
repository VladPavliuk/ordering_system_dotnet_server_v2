using System;
using System.ComponentModel.DataAnnotations;

namespace mvc_auth.Models
{
    public class Service
    {
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public decimal Duration { get; set; }
        public string ImagePath { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}