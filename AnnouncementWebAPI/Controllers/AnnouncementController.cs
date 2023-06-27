using AnnouncementWebAPI.Data;
using AnnouncementWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnnouncementWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public AnnouncementController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAnnouncement()
        {
            var announcements = await dbContext.Announcements.ToListAsync();
            return View(announcements);
        }
        [HttpPost]
        public async Task<IActionResult> AddAnnouncement(Announcement announcement)
        {
            if (announcement == null)
            {
                return BadRequest();
            }
            await dbContext.Announcements.AddAsync(announcement);
            await dbContext.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditAnnouncement(int id, Announcement announcement)
        {
            if (id != announcement.Id)
            {
                return BadRequest();
            }
            dbContext.Announcements.Update(announcement);
            await dbContext.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnnouncement(int id)
        {
            var announcement = await dbContext.Announcements.FindAsync(id);
            if (announcement == null)
            {
                return NotFound();

            }
            dbContext.Announcements.Remove(announcement);
            await dbContext.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// From the entity it will be possible to get: Name, Description, Date Added;
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailsAnnouncement(int? id)
        {
            var announcement = await dbContext.Announcements.FindAsync(id);
            return View(announcement);
        }
        /// <summary>
        /// Top 3 similar announcements
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTop3Similar")]
        public async Task<IActionResult> GetTop3SimilarAnnouncements()
        {
            var charsForSplit = new char[] { ' ', ',', '.', ':', '-', '\t' };
            var announcements = await dbContext.Announcements.ToListAsync();
            var similarAnnouncements = new List<Announcement>();
            foreach (var announcement in announcements)
            {
                var similarWords = new List<string>();
                var titleWords = announcement.Title?.Split(charsForSplit);
                var descriptionWords = announcement.Description?.Split(charsForSplit);

                foreach (var otherAnnouncement in announcements)
                {
                    if (otherAnnouncement.Id == announcement.Id)
                        continue;
                    var otherTitleWords = otherAnnouncement.Title?.Split(charsForSplit);
                    var otherDescriptionWords = otherAnnouncement.Description?.Split(charsForSplit);
                    foreach (var word in titleWords.Union(descriptionWords))
                    {
                        if (otherTitleWords.Contains(word) || otherDescriptionWords.Contains(word))
                        {
                            similarWords.Add(word);
                            break;
                        }
                    }
                }
                if (similarWords.Any())
                    similarAnnouncements.Add(announcement);
            }
            similarAnnouncements = similarAnnouncements.OrderByDescending(a => a.DateAdded).ToList();
            var topAnnouncements = similarAnnouncements.Take(3).ToList();

            return View(topAnnouncements);
        }
    }
}
