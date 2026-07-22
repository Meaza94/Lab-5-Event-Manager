namespace Lab_5_Event_Manager.DTOs
{
    /// <summary>
    /// An event as returned by GET requests, including its list of attendees.
    /// </summary>
    public class EventDto
    {
        /// <summary>Unique identifier of the event.</summary>
        public int Id { get; set; }

        /// <summary>Title of the event.</summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>Optional longer description of the event.</summary>
        public string? Description { get; set; }

        /// <summary>Date and time the event takes place.</summary>
        public DateTime Date { get; set; }

        /// <summary>Where the event is held.</summary>
        public string Location { get; set; } = string.Empty;

        /// <summary>Attendees currently registered for this event.</summary>
        public List<AttendeeDto> Attendees { get; set; } = new();
    }
}
