using Microsoft.EntityFrameworkCore;
using Stocks.Backend.Data;

namespace Stocks.Backend.Extensions;

public static class DatabaseExtensions
{
    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        var services = scope.ServiceProvider;
        try
        {
            var storageContext = services.GetRequiredService<ApplicationDbContext>();
            var pendingMigrations = await storageContext.Database.GetPendingMigrationsAsync();
            var count = pendingMigrations.Count();
            if (count > 0)
            {
                logger.LogInformation("found {Count} pending migrations to apply. will proceed to apply them", count);
                await storageContext.Database.MigrateAsync();
                logger.LogInformation($"done applying pending migrations");
            }
            else
            {
                logger.LogInformation($"no pending migrations found! :)");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while performing migration.");
            throw;
        }
    }
}