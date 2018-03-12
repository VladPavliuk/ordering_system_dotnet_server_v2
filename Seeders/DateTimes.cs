using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mvc_auth.Data;
using mvc_auth.Models;

namespace mvc_auth.Seeders
{
    public class DateTimesInitializare
    {
        private readonly ApplicationDbContext _context;

         List<Date> _dateList = new List<Date>
        {
            new Date()
            {
                Title = "Monday"
            },
            new Date()
            {
                Title = "Tuesday"
            },
            new Date()
            {
                Title = "Wednesday"
            },
            new Date()
            {
                Title = "Thursday"
            },
            new Date()
            {
                Title = "Friday"
            },
            new Date()
            {
                Title = "Saturday"
            },
            new Date()
            {
                Title = "Sunday"
            }
        };

        public DateTimesInitializare(
            ApplicationDbContext context
        )
        {
             _context = context;
        }

         public async Task Seed()
        {
            if (!_context.Date.Any())
            {
                _context.AddRange(_dateList);
                await _context.SaveChangesAsync();
            }
        }
    }
}