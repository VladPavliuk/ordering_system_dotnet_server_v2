using System;

namespace mvc_auth.Models
{
    public class Order
    {
        public int ID { get; set; }
        public User User_ID { get; set; }
        public Organization Organization_ID { get; set; }
        public Service Service_ID { get; set; }
        public decimal Price { get; set; }  
        public DateTime StartedAt { get; set; }
        public DateTime EndedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}