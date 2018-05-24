using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mvc_auth.Data;
using mvc_auth.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Identity;

namespace mvc_auth.Controllers
{
    [Route("api/[controller]")]
    public class AdminsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IServiceProvider serviceProvider
        ) 
        {
            _serviceProvider = serviceProvider;
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        // [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> create([FromBody]JObject data)
        {
             //creating a super user who could maintain the web app
            var poweruser = new ApplicationUser
            {
                UserName = data["name"].ToString(),
                Email = data["name"].ToString()
            };

            string UserPassword = data["password"].ToString();;
            var _user = await _userManager.FindByEmailAsync(data["name"].ToString());
            if(_user == null)
            {
                    var createPowerUser = await _userManager.CreateAsync(poweruser, UserPassword);
                    if (createPowerUser.Succeeded)
                    {
                        //here we tie the new user to the "Admin" role 
                        await _userManager.AddToRoleAsync(poweruser, "Admin");
                    }

                    return Ok();
            }

            return BadRequest();
        }
        
    }
}
