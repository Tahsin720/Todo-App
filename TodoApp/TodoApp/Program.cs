using TodoApp.DependencyServices;
using TodoApp.Middlewares;
using TodoApp.Utiities;

var builder = WebApplication.CreateBuilder(args);


//Dependency Injection
builder.Services.ConfigInjection(builder.Configuration);

//Validation Injection
builder.Services.AddValidationServices();

var app = builder.Build();

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
