using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mvc_auth.Data;
using mvc_auth.Models;

namespace mvc_auth.Controllers
{
    [Route("api/[controller]")]
    public class DaysController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DaysController(
            ApplicationDbContext context
        ) 
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok(_context.Date.ToList());
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
