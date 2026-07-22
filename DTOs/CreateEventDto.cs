using System.ComponentModel.DataAnnotations;

namespace Lab_5_Event_Manager.DTOs
{
    /// <summary>
    /// Fields required to create a new event. Submitted to POST /api/events.
    /// </summary>
    public class CreateEventDto
    {
        /// <summary>Title of the event.</summary>
        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        /// <summary>Optional longer description of the event.</summary>
        [MaxLength(1000)]
        public string? Description { get; set; }

        /// <summary>Date and time the event takes place.</summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>Where the event is held.</summary>
        [Required]
        [MaxLength(200)]
        public string Location { get; set; } = string.Empty;
    }
}
