using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestaurantApi.Data;

namespace RestaurantApi.Tests;

public class TestApiFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = $"TestDb-{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {
            services.AddDbContext<AppDbContext>((sp, options) =>
                options.UseInMemoryDatabase(_dbName)
                       .AddInterceptors(sp.GetRequiredService<AuditInterceptor>()));
        });
    }
}
