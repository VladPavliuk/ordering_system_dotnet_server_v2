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

        [Authorize(Policy = "ApiUser")]
        [HttpPost("pin-service/{organizationId}/{serviceId}")]
        public async Task<IActionResult> PinService(int organizationId, int serviceId)
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

            ApplicationUser user = await _userManager.FindByNameAsync(_userManager.GetUserId(User));
            if(organization.User == user) {
                dbContext.OrganizationServiceRelation.Add(organizationServiceRelation);
                dbContext.SaveChanges();

                return new ObjectResult(organizationServiceRelation);
            }
            return BadRequest();
        } 

        [Authorize(Policy = "ApiUser")]
        [HttpDelete("unpin-service/{organizationId}/{serviceId}")]
        public async Task<IActionResult> UnpinService(int organizationId, int serviceId)
        {
            Organization organization = dbContext.Organization.FirstOrDefault(t => t.ID == organizationId);
            Service service = dbContext.Service.FirstOrDefault(t => t.ID == serviceId);

            if(organization == null || service == null) {
                return NotFound();
            } 

            ApplicationUser user = await _userManager.FindByNameAsync(_userManager.GetUserId(User));
            if(organization.User == user) {
                dbContext.OrganizationServiceRelation.RemoveRange(dbContext.OrganizationServiceRelation.Where(t => t.Organization_ID == organization && t.Service_ID == service));
                dbContext.SaveChanges();

                return Ok();
            }
            return BadRequest();
        }

        [Authorize(Policy = "ApiUser")]
        [HttpGet("{id}/available-services")]
        public async Task<IEnumerable<Service>> OtherSerivcesInOrganization(int id)
        {
            Organization organization = dbContext.Organization.FirstOrDefault(t => t.ID == id);

            if(organization == null) {
                return Enumerable.Empty<Service>();
            }

            ApplicationUser user = await _userManager.FindByNameAsync(_userManager.GetUserId(User));
            if(organization.User == user) {
                return dbContext.Service
                .Where(t => !dbContext.OrganizationServiceRelation
                .Any(b => t.ID == b.Service_ID.ID && organization == b.Organization_ID))
                .ToList();
            }

           return Enumerable.Empty<Service>();
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

        [HttpGet("{id}/orders")]
        [Authorize(Policy = "ApiUser")]
        public IActionResult getOrganizationOrders(int id)
        {
            Organization organization = dbContext.Organization.Where(t => t.ID == id).FirstOrDefault();

            if(organization == null) {
                return NotFound();
            }

            return Ok(dbContext.Order.Where(t => t.Organization_ID == organization).Include(t => t.Service_ID).ToList());
        }

        [HttpGet("owner")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IEnumerable<Organization>> GetAllWhereOwner()
        { 
            ApplicationUser user = await _userManager.FindByNameAsync(_userManager.GetUserId(User));

            return this.dbContext.Organization
                .Where(t => t.User == user)
                .Include(t => t.User)
                .ToList();
        }

        [HttpGet("{id}/is-belong-to-me")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> isOrganizationBelongToAuthUser(int id)
        {
            Organization organization = dbContext.Organization.FirstOrDefault(t => t.ID == id);

            if(organization == null) {
                return NotFound();
            }

            ApplicationUser user = await _userManager.FindByNameAsync(_userManager.GetUserId(User));
            if(organization.User == user) {
                return Ok(true);
            }
            
            return Ok(false);
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

            var schedule = data["schedule"];
            if(schedule != null) {
                JObject[] scheduleList = schedule.ToObject<JObject[]>();

                for(int i = 0; i < scheduleList.Length; i++) {
                    int dateId = scheduleList[i]["id"].ToObject<int>();
                    Date date = dbContext.Date.Where(t => t.ID == dateId).FirstOrDefault();
                    if( date != null) {
                        OrganizationDateRelation organizationDateRelation = new OrganizationDateRelation {
                            Organization_ID = organization,
                            Date_ID = date,
                            From = scheduleList[i]["from"].ToObject<TimeSpan>(),
                            To = scheduleList[i]["to"].ToObject<TimeSpan>(),
                            IsDayAndNight = scheduleList[i]["isAllDayAndNight"] == null ? false : scheduleList[i]["isAllDayAndNight"].ToObject<bool>(),
                            IsHoliday = scheduleList[i]["isHoliday"] == null ? false : scheduleList[i]["isHoliday"].ToObject<bool>()
                        };

                        dbContext.OrganizationDateRelation.Add(organizationDateRelation);
                    }
                    
                }
            }
            
           dbContext.SaveChanges();

            return Ok();
        }

        [Authorize(Policy = "ApiUser")]
        [HttpPost("{id}/set-schedule")]
        public IActionResult setSchedule(long id, [FromBody]JObject data)
        {
            DateTime from = DateTime.Parse(data["from"].ToObject<String>());
            DateTime to = DateTime.Parse(data["to"].ToObject<String>());
            Date dateToSet = dbContext.Date.FirstOrDefault(t => t.ID == data["dateId"].ToObject<int>());
            Organization organization = dbContext.Organization.FirstOrDefault(t => t.ID == id);

            if(dateToSet == null || organization == null) {
                 return BadRequest();
            } 

            dbContext.OrganizationDateRelation.Add(new OrganizationDateRelation(){
                Date_ID = dateToSet,
                From = data["from"].ToObject<TimeSpan>(),
                To = data["to"].ToObject<TimeSpan>(),
                IsDayAndNight = data["isDayAndNight"] == null ? data["isDayAndNight"].ToObject<bool>() : false,
                IsHoliday = data["isHoliday"] == null ? data["isHoliday"].ToObject<bool>() : false,
                Organization_ID = organization
            });
            dbContext.SaveChanges();

            return Ok();
        }

        [HttpGet("{organization_id}/{service_id}/mark")]
        public IActionResult getAverageMark(int organization_id, int service_id)
        {
            Organization organization = dbContext.Organization.Where(t => t.ID == organization_id).FirstOrDefault();
            Service service = dbContext.Service.Where(t => t.ID == service_id).FirstOrDefault();
            List<Order> orders = dbContext.Order
                .Include(t => t.Organization_ID)
                .Include(t => t.Service_ID)
                .Where(t => t.Organization_ID == organization && t.Service_ID == service).ToList();

            if(organization == null || service == null || orders == null) {
                return BadRequest();
            }

            List<decimal> organizationRating = new List<decimal>();

            foreach(var order in orders) {
                organizationRating.Add(dbContext.OrganizationMarkup.Where(t => t.Order_ID == order).Select(t => t.Value).FirstOrDefault());
            }
            
            if(!organizationRating.Any() || organizationRating == null) {
                return Ok();
            }

            return Ok(organizationRating.Average());
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

        [HttpGet("{id}/is-available/{requestDateFrom}/{requestDateTo}")]
        public IActionResult isAvailable(long id, string requestDateFrom, string requestDateTo)
        {
            DateTime fromDateToCheck = DateTime.Parse(requestDateFrom);
            DateTime toDateToCheck = DateTime.Parse(requestDateTo);
            Date dateOfWeek = dbContext.Date.First(t => t.Title == fromDateToCheck.DayOfWeek.ToString());
            Organization organization = dbContext.Organization.FirstOrDefault(t => t.ID == id);

            if(organization == null) {
                return NotFound();
            } else if(dateOfWeek == null || fromDateToCheck == null || toDateToCheck == null) {
                return BadRequest();
            }

            OrganizationDateRelation organizationScheduleAtDay = dbContext.OrganizationDateRelation
                .FirstOrDefault(t => t.Organization_ID == organization && t.Date_ID == dateOfWeek);

            TimeSpan fromTimeToCheck = TimeSpan.Parse(fromDateToCheck.ToString("HH:mm"));
            TimeSpan toTimeToCheck = TimeSpan.Parse(toDateToCheck.ToString("HH:mm"));
            Tuple<TimeSpan, TimeSpan> scheduleRange = new Tuple<TimeSpan, TimeSpan>((TimeSpan) organizationScheduleAtDay.From, (TimeSpan) organizationScheduleAtDay.To);
            Tuple<TimeSpan, TimeSpan> requestRange = new Tuple<TimeSpan, TimeSpan>(fromTimeToCheck, toTimeToCheck);

            return Ok(
                !(scheduleRange.Item1 > scheduleRange.Item2 || requestRange.Item1 > requestRange.Item2) &&
                (scheduleRange.Item1 <= requestRange.Item1 && requestRange.Item1 <= scheduleRange.Item2
                && scheduleRange.Item1 <= requestRange.Item2 && requestRange.Item2 <= scheduleRange.Item2)
            );
        }

        [Authorize(Policy = "ApiUser")]
        [HttpPost("{id}/set-avatar")]
        public async Task<IActionResult> SetAvatar(long id, IFormFile Image)
        {
            Organization organization = dbContext.Organization.FirstOrDefault(t => t.ID == (int) id);
            ApplicationUser user = await _userManager.FindByNameAsync(_userManager.GetUserId(User));

            if (Image!= null && organization.User == user)
            {
                string fileName = RandomString(30);
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
                    await Image.CopyToAsync(fs);

                    organization.ImagePath = "/images/organizations/" + fileName;
                    dbContext.Organization.Update(organization);
                    dbContext.SaveChanges();
                }
                
                return Ok();   
            }

            return BadRequest();
        }

        private string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch ;
            for(int i=0; i<size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65))) ;
                builder.Append(ch);
            }
            return builder.ToString();
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