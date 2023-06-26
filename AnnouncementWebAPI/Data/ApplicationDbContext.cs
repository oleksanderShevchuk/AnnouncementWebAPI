using AnnouncementWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AnnouncementWebAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Announcement> Announcements { get; set; }
    }
}
