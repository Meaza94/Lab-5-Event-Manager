using Lab_5_Event_Manager.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab_5_Event_Manager.Data
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            // Apply any pending EF Core migrations (creates the DB if it doesn't exist yet).
            context.Database.Migrate();

            // Already seeded.
            if (context.Events.Any())
            {
                return;
            }

            var events = new List<Event>
            {
                new Event
                {
                    Title = "Tech Career Fair",
                    Description = "Meet employers hiring for co-op and grad roles.",
                    Date = DateTime.Now.AddDays(14),
                    Location = "Algonquin College, Ottawa",
                    Attendees = new List<Attendee>
                    {
                        new Attendee { Name = "Alice Nguyen", Email = "alice.nguyen@example.com" },
                        new Attendee { Name = "Brian Osei", Email = "brian.osei@example.com" }
                    }
                },
                new Event
                {
                    Title = "ASP.NET Core Workshop",
                    Description = "Hands-on workshop building Web APIs with EF Core.",
                    Date = DateTime.Now.AddDays(21),
                    Location = "Room T110, Woodroffe Campus",
                    Attendees = new List<Attendee>
                    {
                        new Attendee { Name = "Chloe Martin", Email = "chloe.martin@example.com" },
                        new Attendee { Name = "David Lee", Email = "david.lee@example.com" }
                    }
                },
                new Event
                {
                    Title = "Alumni Networking Night",
                    Description = "Connect with CST program alumni working in industry.",
                    Date = DateTime.Now.AddDays(30),
                    Location = "Ottawa Conference Centre",
                    Attendees = new List<Attendee>
                    {
                        new Attendee { Name = "Emma Wilson", Email = "emma.wilson@example.com" },
                        new Attendee { Name = "Farid Haidari", Email = "farid.haidari@example.com" }
                    }
                }
            };

            context.Events.AddRange(events);
            context.SaveChanges();
        }
    }
}
