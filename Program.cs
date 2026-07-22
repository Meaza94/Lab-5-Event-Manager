using Lab_5_Event_Manager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// EF Core + SQL Server (LocalDB)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Event Manager API",
        Version = "v1",
        Description = "A RESTful API for managing Events and their Attendees. " +
                      "Create, read, update, and delete events, and register or unregister attendees from an event."
    });

    // Group endpoints under "Events" / "Attendees" using each action's [Tags] attribute
    // instead of the default (one group per controller).
    c.TagActionsBy(api =>
    {
        var tag = api.ActionDescriptor.EndpointMetadata
            .OfType<Microsoft.AspNetCore.Http.TagsAttribute>()
            .FirstOrDefault()?.Tags.FirstOrDefault();

        return new[] { tag ?? api.ActionDescriptor.RouteValues["controller"] ?? "Default" };
    });
    c.DocInclusionPredicate((docName, api) => true);

    // Pull in the /// XML summary comments on controllers and DTOs so Swagger shows real descriptions.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Apply migrations + seed data on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    DbInitializer.Seed(db);
}

// Serve the custom Swagger stylesheet from wwwroot
app.UseStaticFiles();

// Swagger UI (available in all environments for this lab)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Event Manager API v1");
    c.DocumentTitle = "Event Manager API";
    c.InjectStylesheet("/swagger-ui/custom.css");
    c.DefaultModelsExpandDepth(-1); // hide the schemas section by default, cleaner first look
    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
});

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
