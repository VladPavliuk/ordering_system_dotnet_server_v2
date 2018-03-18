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

namespace Reservation.Controllers
{   
    [Route("api/[controller]")]
    public class OrganizationsController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrganizationsController(
            ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager
        )
        {
            _userManager = userManager;
            this.dbContext = dbContext;
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
            .Include(t => t.User)
            .ToList();
        }

        [HttpPost]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> Create([FromBody]JObject data)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(_userManager.GetUserId(User));

            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Organization organization = new Organization();
            organization.User = user;
            organization.Title = data["title"].ToString();
            
            dbContext.Organization.Add(organization);
            dbContext.SaveChanges();

            return Ok();
        }

        [HttpPost("{id}/set-schedule")]
        public IActionResult setSchedule(long id, [FromBody]JObject data)
        {
            DateTime from = DateTime.Parse(data["from"].ToObject<String>());
            DateTime to = DateTime.Parse(data["to"].ToObject<String>());
            Date dateToSet = dbContext.Date.FirstOrDefault(t => t.Title == data["date"].ToObject<string>());
            Organization organization = dbContext.Organization.FirstOrDefault(t => t.ID == id);

            if(dateToSet == null || organization == null) {
                 return BadRequest();
            } 

             dbContext.OrganizationDateRelation.Add(new OrganizationDateRelation(){
                Date_ID = dateToSet,
                From = from,
                To = to,
                Organization_ID = organization
            });
            dbContext.SaveChanges();

            return Ok();
        }

        [HttpGet("{id}/get-schedule")]
        public IActionResult getSchedule(long id)
        {
            Organization organization = dbContext.Organization.FirstOrDefault(t => t.ID == id);
            List<OrganizationDateRelation> schedule = dbContext.OrganizationDateRelation
                .Where(t => t.Organization_ID == organization)
                .Include(t => t.Organization_ID)
                .Include(t => t.Date_ID)
                .ToList();

            return Ok(schedule);
        }

        [HttpGet("{id}/is-available/{requestDateToCheck}")]
        public IActionResult isAvailable(long id, string requestDateToCheck)
        {
            DateTime dateToCheck = DateTime.Parse(requestDateToCheck);
            Date dateOfWeek = dbContext.Date.First(t => t.Title == dateToCheck.DayOfWeek.ToString());
            Organization organization = dbContext.Organization.FirstOrDefault(t => t.ID == id);

            OrganizationDateRelation organizationScheduleAtDay = dbContext.OrganizationDateRelation
                .FirstOrDefault(t => t.Organization_ID == organization && t.Date_ID == dateOfWeek);

            string tmpRequestDate = dateToCheck.ToString("H:mm");
            string tmpDateBaseDateFrom = organizationScheduleAtDay.From.ToString("H:mm");
            string tmpDateBaseDateTo = organizationScheduleAtDay.To.ToString("H:mm");

            bool result = DateTime.Parse(tmpRequestDate) > DateTime.Parse(tmpDateBaseDateFrom) && DateTime.Parse(tmpRequestDate) < DateTime.Parse(tmpDateBaseDateTo);
            return Ok(result);
        }

        [HttpPost("{id}/set-avatar")]
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
                .Include(t => t.User)
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