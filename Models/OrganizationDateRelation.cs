using System;
using System.ComponentModel.DataAnnotations;

namespace mvc_auth.Models
{
    public class OrganizationDateRelation
    {
        public int ID { get; set; }
        public Organization Organization_ID { get; set; }
        public Date Date_ID { get; set; }
        public TimeSpan? From { get; set; }
        public TimeSpan? To { get; set; }
        public bool? IsDayAndNight { get; set; } = false;
        public bool? IsHoliday { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}