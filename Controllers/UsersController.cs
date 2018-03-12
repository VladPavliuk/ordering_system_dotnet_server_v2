using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using mvc_auth.Models;
using mvc_auth.Data;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Reservation.Controllers
{   
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public UsersController(
            ApplicationDbContext dbContext
        )
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IEnumerable<User> GetAll()
        { 
            return dbContext.User.ToList();
        }

        [HttpPost]
        public IActionResult Create([FromBody] User userTostore)
        {
            User user = new User { FirstName = userTostore.FirstName, LastName = userTostore.LastName, Phone = userTostore.Phone };

            dbContext.User.Add(user);
            dbContext.SaveChanges();

            return Ok();
        }

    //     [HttpPut("{id}")]
    //     public IActionResult Update(long id, [FromBody] User user)
    //     {
    //         var userToUpdate = dbContext.User.FirstOrDefault(t => t.Id == id);
    //         if (userToUpdate == null)
    //         {
    //             return NotFound();
    //         }

    //         userToUpdate.FirstName = user.FirstName;
    //         userToUpdate.LastName = user.LastName;
    //         userToUpdate.Phone = user.Phone;
    //         userToUpdate.Email = user.Email;
    //         user.Id = (int) id;

    //         dbContext.User.Update(user);
    //         dbContext.SaveChanges();

    //         return new ObjectResult(user);
    //     }

    //     [HttpGet("{id}")]
    //     public User GetById(long id)
    //     {
    //         return dbContext.User.FirstOrDefault(t => t.Id == id);
    //     }

    //     [HttpDelete("{id}")]
    //     public IActionResult Delete(long id)
    //     {
    //         var user = dbContext.User.FirstOrDefault(t => t.Id == id);
    //         if (user == null)
    //         {
    //             return BadRequest();
    //         }

    //         dbContext.User.Remove(user);
    //         dbContext.SaveChanges();
    //         return Ok();
    //     }
    }
}