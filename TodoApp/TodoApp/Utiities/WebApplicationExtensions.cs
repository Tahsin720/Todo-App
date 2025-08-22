using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Database;
using TodoApp.Domain.Entities;

namespace TodoApp.Utiities;

public static class WebApplicationExtensions
{
    public static WebApplication ApplyDatabaseMigrations(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
            return app;
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Apply pending migrations
        db.Database.Migrate();

        return app;
    }
}
