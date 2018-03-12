using System;

namespace mvc_auth.Models
{
    public class OrganizationServiceRelation
    {
        public int ID { get; set; }
        public Organization Organization_ID { get; set; }
        public Service Service_ID { get; set; }
        public decimal Price { get; set; }
        public decimal Duration { get; set; }
         public string ImagePath { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}