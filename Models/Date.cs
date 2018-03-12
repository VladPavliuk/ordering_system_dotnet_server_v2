using System;
using System.ComponentModel.DataAnnotations;

namespace mvc_auth.Models
{
    public class Date
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}