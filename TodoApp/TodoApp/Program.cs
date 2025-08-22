using Microsoft.EntityFrameworkCore;
using TodoApp.DependencyServices;
using TodoApp.Domain.Database;
using TodoApp.Middlewares;
using TodoApp.Utiities;

var builder = WebApplication.CreateBuilder(args);


//Dependency Injection
builder.Services.ConfigInjection(builder.Configuration);

//Validation Injection
builder.Services.AddValidationServices();

var app = builder.Build();

// Auto-migrate and update database on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyDatabaseMigrations();
}

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseMiddleware<CustomExceptionMiddleware>();

app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.Run();
