using System.ComponentModel.DataAnnotations;

namespace Lab_5_Event_Manager.DTOs
{
    /// <summary>
    /// Fields required to register an attendee. Submitted to POST /api/events/{eventId}/attendees.
    /// </summary>
    public class RegisterAttendeeDto
    {
        /// <summary>Full name of the attendee.</summary>
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        /// <summary>Email address of the attendee.</summary>
        [Required]
        [EmailAddress]
        [MaxLength(200)]
        public string Email { get; set; } = string.Empty;
    }
}
