using System.ComponentModel.DataAnnotations;

namespace Lab_5_Event_Manager.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(200)]
        public string Location { get; set; } = string.Empty;

        // Navigation property - one Event has many Attendees
        public List<Attendee> Attendees { get; set; } = new();
    }
}
