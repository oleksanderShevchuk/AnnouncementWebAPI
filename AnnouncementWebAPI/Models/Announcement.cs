using System.ComponentModel.DataAnnotations;

namespace AnnouncementWebAPI.Models
{
    public class Announcement
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
