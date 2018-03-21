using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mvc_auth.Data;
using mvc_auth.Models;

namespace mvc_auth.Seeders
{
    public class ServicesInitializare
    {
        private readonly ApplicationDbContext _context;

         List<Service> _dateList = new List<Service>
        {
            new Service()
            {
                Title = "Restaurants",
                ImagePath = "/images/services/order_a_table.svg",
                Price = 100,
                Duration = 30
            },
            new Service()
            {
                Title = "Taxi",
                ImagePath = "/images/services/taxi.svg",
                Price = 50,
                Duration = 15
            },
            new Service()
            {
                Title = "Coffe",
                ImagePath = "/images/services/coffee.svg",
                Price = 10,
                Duration = 10
            },
            new Service()
            {
                Title = "Medicine",
                ImagePath = "/images/services/medkit.svg",
                Price = 300,
                Duration = 60
            },
            new Service()
            {
                Title = "Bank",
                ImagePath = "/images/services/dollar-sign.svg",
                Price = 20,
                Duration = 10000
            },
            new Service()
            {
                Title = "Support",
                ImagePath = "/images/services/support.svg",
                Price = 5,
                Duration = 30
            },
            new Service()
            {
                Title = "Hotel",
                ImagePath = "/images/services/hotel.svg",
                Price = 150,
                Duration = 1200
            },
            new Service()
            {
                Title = "Transport",
                ImagePath = "/images/services/transport.svg",
                Price = 500,
                Duration = 60
            },
            new Service()
            {
                Title = "Beer",
                ImagePath = "/images/services/beer.svg",
                Price = 30,
                Duration = 180
            }
        };

        public ServicesInitializare(
            ApplicationDbContext context
        )
        {
             _context = context;
        }

         public async Task Seed()
        {
            if (!_context.Service.Any())
            {
                _context.AddRange(_dateList);
                await _context.SaveChangesAsync();
            }
        }
    }
}