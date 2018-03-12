using System;
using System.ComponentModel.DataAnnotations;

namespace mvc_auth.Models
{
    public class Admin
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        [MaxLength(50)] 
        public string Phone { get; set; }
        [Required]
        [MaxLength(50)] 
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [MaxLength(255)]
        public string Token { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}