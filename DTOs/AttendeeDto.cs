namespace Lab_5_Event_Manager.DTOs
{
    /// <summary>
    /// An attendee as returned inside an EventDto.
    /// </summary>
    public class AttendeeDto
    {
        /// <summary>Unique identifier of the attendee.</summary>
        public int Id { get; set; }

        /// <summary>Full name of the attendee.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Email address of the attendee.</summary>
        public string Email { get; set; } = string.Empty;
    }
}
