using System;
using System.ComponentModel.DataAnnotations;

namespace mvc_auth.Models
{
    public class Date
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public TimeSpan? From { get; set; } = TimeSpan.Parse("09:00");
        public TimeSpan? To { get; set; } = TimeSpan.Parse("18:00");
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}