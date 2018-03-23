using System;

namespace mvc_auth.Models
{
    public class OrganizationRating
    {
        public int ID { get; set; }
        public Order Order_ID { get; set; }
        public ApplicationUser User_ID { get; set; }
        public decimal Value { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}