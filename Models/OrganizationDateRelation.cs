using System;
using System.ComponentModel.DataAnnotations;

namespace mvc_auth.Models
{
    public class OrganizationDateRelation
    {
        public int ID { get; set; }
        public Organization Organization_ID { get; set; }
        public Date Date_ID { get; set; }

        [DisplayFormat(DataFormatString = "{0:H:mm}")]
        [Required]
        public DateTime From { get; set; }

        [DisplayFormat(DataFormatString = "{0:H:mm}")]
        [Required]
        public DateTime To { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}