using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using mvc_auth.Models;
using mvc_auth.Data;
using System;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Text;

namespace Reservation.Controllers
{   
    [Route("api/[controller]")]
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public ServicesController(
            ApplicationDbContext dbContext
        )
        {
            this.dbContext = dbContext;
        }

        [HttpGet("{id}/organizations")]
        public IEnumerable<OrganizationServiceRelation> GetOrganizationsBelongToService(int id)
        {
            Service service = dbContext.Service.FirstOrDefault(t => t.ID == id);

            if(service == null) {
                return Enumerable.Empty<OrganizationServiceRelation>();
            }

            return dbContext.OrganizationServiceRelation
                .Include(t => t.Service_ID)
                .Include(t => t.Organization_ID)
                .Where(t => t.Service_ID == service)
                .ToList();
        }

        [HttpGet]
        public IEnumerable<Service> GetAll()
        { 
            return this.dbContext.Service.ToList();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public bool Create([FromBody] Service service)
        {
            service.CreatedAt = DateTime.Now;

            dbContext.Service.Add(service);
            dbContext.SaveChanges();

            return true;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(long id, [FromBody] Service service)
        {
            var serviceToUpdate = dbContext.Service.FirstOrDefault(t => t.ID == id);
            if (serviceToUpdate == null)
            {
                return NotFound();
            }

            serviceToUpdate.Title = service.Title;
            service.ID = (int) id;

            dbContext.Service.Update(serviceToUpdate);
            dbContext.SaveChanges();

            return new ObjectResult(service);
        }

        [HttpGet("{id}")]
        public Service GetById(long id)
        {
            return dbContext.Service.FirstOrDefault(t => t.ID == id);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public bool Delete(long id)
        {
            var service = dbContext.Service.FirstOrDefault(t => t.ID == id);
            if (service == null)
            {
                return false;
            }

            dbContext.Service.Remove(service);
            dbContext.SaveChanges();
            return true;
        }
    }
}