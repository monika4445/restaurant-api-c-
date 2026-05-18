using Microsoft.EntityFrameworkCore;
using RestaurantApi.Configuration;
using RestaurantApi.Data;
using RestaurantApi.Middleware;
using RestaurantApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<AuditInterceptor>();

if (!builder.Environment.IsEnvironment("Test"))
{
    var connectionString = builder.Configuration.GetConnectionString("Default")
        ?? throw new InvalidOperationException("Missing ConnectionStrings:Default. Set it via user-secrets or appsettings.");

    builder.Services.AddDbContext<AppDbContext>((sp, options) =>
        options.UseNpgsql(connectionString)
               .AddInterceptors(sp.GetRequiredService<AuditInterceptor>()));
}

builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IMembershipService, MembershipService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddHealthChecks();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "RestaurantApi",
        Version = "v1",
        Description = "Restaurants, Players, Memberships and Favorites. " +
                      "Built on ASP.NET Core (.NET 10) with PostgreSQL via EF Core. " +
                      "All save endpoints enforce duplicate detection (case-insensitive, race-safe). " +
                      "Retrieve endpoints support partial search where the spec says so. " +
                      "\n\n**Tip:** every \"Try it out\" request body is pre-filled with valid sample values — " +
                      "click Execute as-is for a happy-path 201. Modify any field to test invalid cases (400 / 409 / 404).",
        Contact = new() { Name = "Source", Url = new Uri("https://github.com/monika4445/restaurant-api-c-") }
    });
    options.SchemaFilter<DefaultValueExampleFilter>();
    options.ParameterFilter<DefaultValueParameterFilter>();
});

var app = builder.Build();

var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(port))
{
    app.Urls.Clear();
    app.Urls.Add($"http://+:{port}");
}

app.UseExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "RestaurantApi v1");
});

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapControllers();

app.Run();

public partial class Program { }
