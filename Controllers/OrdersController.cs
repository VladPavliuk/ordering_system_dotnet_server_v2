using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using mvc_auth.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using mvc_auth.Data;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace mvc_auth.Controllers
{   
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
         private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrdersController(
            ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager
        )
        {
            this.dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpGet("auth-user-orders")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IEnumerable<Order>> authUserOrdersList()
        {
            ApplicationUser user = await _userManager.FindByNameAsync(_userManager.GetUserId(User));

            return dbContext.Order.Where(t => t.User_ID == user)
                .Include(t => t.Service_ID)
                .Include(t => t.Organization_ID)
                .ToList();
        }

        [HttpPost]
        [Authorize(Policy = "ApiUser")]
        [Route("make-order")]
        public async Task<IActionResult> makeOrder([FromBody]JObject data)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(_userManager.GetUserId(User));
            Organization organization = dbContext.Organization.FirstOrDefault(t => t.ID == data["Organization_ID"].ToObject<int>());
            Service service = dbContext.Service.FirstOrDefault(t => t.ID == data["Service_ID"].ToObject<int>());

            if(organization == null || service == null) {
                return NotFound();
            } 

            dbContext.Order.Add(new Order() { 
                Organization_ID = organization,
                Service_ID = service,
                Price = data["Price"].ToObject<int>(),
                User_ID = user,
                StartedAt = data["StartedAt"].ToObject<DateTime>(),
                EndedAt = data["StartedAt"].ToObject<DateTime>().AddMinutes(data["Duration"].ToObject<int>()),
            });
            dbContext.SaveChanges();
            
            return Ok();
        }

        public IEnumerable<Order> index()
        {
            return dbContext
                .Order
                .Include(t => t.Service_ID)
                .Include(t => t.Organization_ID)
                .Include(t => t.User_ID)
                .OrderByDescending(t => t.EndedAt)
                .ToList();
        }
    }
}