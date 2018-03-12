using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Reservation.Models;
using System;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Reservation.Controllers
{   
    [Route("api/[controller]")]
    public class OrganizationsController : Controller
    {
        private readonly ReservationDbContext dbContext;

        public OrganizationsController()
        {
            this.dbContext = new ReservationDbContext();
        }

        [HttpPost("pin-service/{organizationId}/{serviceId}")]
        public IActionResult PinService(int organizationId, int serviceId)
        {
            Organization organization = dbContext.Organization.FirstOrDefault(t => t.ID == organizationId);
            Service service = dbContext.Service.FirstOrDefault(t => t.ID == serviceId);

            if(organization == null || service == null) {
                return NotFound();
            } 

            OrganizationServiceRelation organizationServiceRelation = new OrganizationServiceRelation() {
                Organization_ID = organization,
                Service_ID = service,
                Price = service.Price,
                Duration = service.Duration
            };

            dbContext.OrganizationServiceRelation.Add(organizationServiceRelation);
            dbContext.SaveChanges();

            return new ObjectResult(organizationServiceRelation);
        } 

        [HttpDelete("unpin-service/{organizationId}/{serviceId}")]
        public IActionResult UnpinService(int organizationId, int serviceId)
        {
            Organization organization = dbContext.Organization.FirstOrDefault(t => t.ID == organizationId);
            Service service = dbContext.Service.FirstOrDefault(t => t.ID == serviceId);

            if(organization == null || service == null) {
                return NotFound();
            } 

            dbContext.OrganizationServiceRelation.RemoveRange(
                dbContext.OrganizationServiceRelation.Where(t => t.Organization_ID == organization && t.Service_ID == service)
            );
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpGet("{id}/available-services")]
        public IEnumerable<Service> OtherSerivcesInOrganization(int id)
        {
            Organization organization = dbContext.Organization.FirstOrDefault(t => t.ID == id);

            if(organization == null) {
                return Enumerable.Empty<Service>();
            }

            return dbContext.Service
                .Where(t => !dbContext.OrganizationServiceRelation
                    .Any(b => t.ID == b.Service_ID.ID && organization == b.Organization_ID))
                .ToList();
        }

        [HttpGet("{id}/services-list")]
        public IEnumerable<OrganizationServiceRelation> ServicesList(int id)
        {
            Organization organization = dbContext.Organization.FirstOrDefault(t => t.ID == id);

            if(organization == null) {
                return Enumerable.Empty<OrganizationServiceRelation>();
            }

            return dbContext.OrganizationServiceRelation
                .Include(t => t.Service_ID)
                .Include(t => t.Organization_ID)
                .Where(b => b.Organization_ID == organization)
                .ToList();
        }

        [HttpGet]
        public IEnumerable<Organization> GetAll()
        { 
            return this.dbContext.Organization
            .Include(t => t.User_ID)
            .ToList();
        }

        [HttpPost]
        public IActionResult Create([FromBody]JObject data)
        {
            int userId = data["user_id"].ToObject<int>();
            
            User user = dbContext.User.FirstOrDefault(t => t.Id == userId);
            if(user == null) {
                return NotFound();
            }
            Organization organization = new Organization();
            organization.User_ID = user;
            organization.Title = data["title"].ToString();
            organization.Schedule = data["schedule"].ToString();

            dbContext.Organization.Add(organization);
            dbContext.SaveChanges();

            return Ok();
        }

        [HttpPost("set-avatar/{id}")]
        public IActionResult SetAvatar(long id, IFormFile Image)
        {
            Organization organization = dbContext.Organization.FirstOrDefault(t => t.ID == (int) id);
            if (Image!= null)
            {
                string fileName = organization.ID + "_" + organization.Title + "_" + DateTime.Now.ToString("y_M_d");
                switch(Image.ContentType) {
                    case "image/jpeg": {
                        fileName += ".jpeg";
                        break;
                    }
                    case "image/png": {
                        fileName += ".png";
                        break;
                    }
                    default: {
                        return BadRequest();
                    }
                }
                
                using (FileStream fs = System.IO.File.Create("wwwroot/images/organizations/" + fileName))
                {
                    Image.CopyToAsync(fs);

                    organization.ImagePath = "wwwroot/images/organizations/" + fileName;
                    dbContext.Organization.Update(organization);
                    dbContext.SaveChanges();
                }
                
                return Ok();   
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Organization organization)
        {
            Organization organizationToUpdate = dbContext.Organization.FirstOrDefault(t => t.ID == id);
            if (organizationToUpdate == null)
            {
                return NotFound();
            }

            //organizationToUpdate.Title = organization.Title;
            // organizationToUpdate.ID = (int) id;

            dbContext.Organization.Update(organizationToUpdate);
            dbContext.SaveChanges();

            return new ObjectResult(organizationToUpdate);
        }

        [HttpGet("{id}")]
        public Organization GetById(long id)
        {
            return dbContext.Organization
                .Include(t => t.User_ID)
                .FirstOrDefault(t => t.ID == id);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var organization = dbContext.Organization.FirstOrDefault(t => t.ID == id);
            if (organization == null)
            {
                return BadRequest();
            }

            dbContext.Organization.Remove(organization);
            dbContext.SaveChanges();
            return Ok();
        }
    }
}