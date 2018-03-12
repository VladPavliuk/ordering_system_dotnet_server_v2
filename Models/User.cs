using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace mvc_auth.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [MaxLength(50)] 
        public string Phone { get; set; }
        // [Required]
        // [MaxLength(50)] 
        // [DataType(DataType.Password)]
        // public string Password { get; set; }
        // [MaxLength(255)]
        // public string Email { get; set; }
        // public string Token { get; set; }
        public string ImagePath { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        

    }
}