using System;

namespace mvc_auth.Models
{
    public class OrganizationRating
    {
        public int ID { get; set; }
        public OrganizationServiceRelation Organization_Service_ID { get; set; }
        public User User_ID { get; set; }
        public decimal Value { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}