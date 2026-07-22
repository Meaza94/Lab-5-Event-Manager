using Lab_5_Event_Manager.Data;
using Lab_5_Event_Manager.DTOs;
using Lab_5_Event_Manager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab_5_Event_Manager.Controllers
{
    /// <summary>
    /// Manages Events and their Attendees.
    /// </summary>
    [ApiController]
    [Route("api/events")]
    [Produces("application/json")]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get every event, including its registered attendees.
        /// </summary>
        [HttpGet]
        [Tags("Events")]
        [ProducesResponseType(typeof(IEnumerable<EventDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents()
        {
            var events = await _context.Events
                .Include(e => e.Attendees)
                .AsNoTracking()
                .ToListAsync();

            return Ok(events.Select(ToEventDto));
        }

        /// <summary>
        /// Get a single event by id, including its registered attendees.
        /// </summary>
        /// <param name="id">The event's id.</param>
        [HttpGet("{id}")]
        [Tags("Events")]
        [ProducesResponseType(typeof(EventDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EventDto>> GetEvent(int id)
        {
            var ev = await _context.Events
                .Include(e => e.Attendees)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (ev == null)
            {
                return NotFound();
            }

            return Ok(ToEventDto(ev));
        }

        /// <summary>
        /// Create a new event.
        /// </summary>
        [HttpPost]
        [Tags("Events")]
        [ProducesResponseType(typeof(EventDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EventDto>> CreateEvent(CreateEventDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ev = new Event
            {
                Title = dto.Title,
                Description = dto.Description,
                Date = dto.Date,
                Location = dto.Location
            };

            _context.Events.Add(ev);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = ev.Id }, ToEventDto(ev));
        }

        /// <summary>
        /// Update an existing event's details.
        /// </summary>
        /// <param name="id">The event's id.</param>
        /// <param name="dto">The updated event fields.</param>
        [HttpPut("{id}")]
        [Tags("Events")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateEvent(int id, UpdateEventDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ev = await _context.Events.FindAsync(id);
            if (ev == null)
            {
                return NotFound();
            }

            ev.Title = dto.Title;
            ev.Description = dto.Description;
            ev.Date = dto.Date;
            ev.Location = dto.Location;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Delete an event. All of its attendees are deleted too (cascade delete).
        /// </summary>
        /// <param name="id">The event's id.</param>
        [HttpDelete("{id}")]
        [Tags("Events")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev == null)
            {
                return NotFound();
            }

            _context.Events.Remove(ev);
            await _context.SaveChangesAsync(); // cascade delete removes Attendees too

            return NoContent();
        }

        /// <summary>
        /// Register a new attendee for an event.
        /// </summary>
        /// <param name="eventId">The event to register the attendee to.</param>
        /// <param name="dto">The attendee's name and email.</param>
        [HttpPost("{eventId}/attendees")]
        [Tags("Attendees")]
        [ProducesResponseType(typeof(AttendeeDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AttendeeDto>> RegisterAttendee(int eventId, RegisterAttendeeDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ev = await _context.Events.FindAsync(eventId);
            if (ev == null)
            {
                return NotFound();
            }

            var attendee = new Attendee
            {
                Name = dto.Name,
                Email = dto.Email,
                EventId = eventId
            };

            _context.Attendees.Add(attendee);
            await _context.SaveChangesAsync();

            var attendeeDto = new AttendeeDto
            {
                Id = attendee.Id,
                Name = attendee.Name,
                Email = attendee.Email
            };

            return CreatedAtAction(nameof(GetEvent), new { id = eventId }, attendeeDto);
        }

        /// <summary>
        /// Unregister (remove) an attendee from an event.
        /// </summary>
        /// <param name="eventId">The event the attendee belongs to.</param>
        /// <param name="attendeeId">The attendee to remove.</param>
        [HttpDelete("{eventId}/attendees/{attendeeId}")]
        [Tags("Attendees")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UnregisterAttendee(int eventId, int attendeeId)
        {
            var ev = await _context.Events.FindAsync(eventId);
            if (ev == null)
            {
                return NotFound();
            }

            var attendee = await _context.Attendees
                .FirstOrDefaultAsync(a => a.Id == attendeeId && a.EventId == eventId);

            if (attendee == null)
            {
                return NotFound();
            }

            _context.Attendees.Remove(attendee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private static EventDto ToEventDto(Event ev)
        {
            return new EventDto
            {
                Id = ev.Id,
                Title = ev.Title,
                Description = ev.Description,
                Date = ev.Date,
                Location = ev.Location,
                Attendees = ev.Attendees.Select(a => new AttendeeDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Email = a.Email
                }).ToList()
            };
        }
    }
}
