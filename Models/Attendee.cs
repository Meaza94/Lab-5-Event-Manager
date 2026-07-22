using System.ComponentModel.DataAnnotations;

namespace Lab_5_Event_Manager.Models
{
    public class Attendee
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        // Foreign key
        public int EventId { get; set; }

        // Navigation property - many Attendees belong to one Event
        public Event? Event { get; set; }
    }
}
