using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using TodoApp.Domain.Database;
using TodoApp.Domain.Entities;
using TodoApp.Services;

namespace TodoApp.DependencyServices;

public static class DependencyInjection
{
    public static IServiceCollection ConfigInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServices(configuration);

        #region Swagger
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Todo App API",
                Version = "v1"
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
               {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
               });
        });
        #endregion

        #region Authentication
        services.AddIdentity<User, IdentityRole>(x =>
        {
            x.Password.RequiredLength = 4;
            x.Password.RequireNonAlphanumeric = false;
            x.Password.RequireUppercase = false;
            x.Password.RequireLowercase = false;
            x.Password.RequireDigit = false;
        }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(options =>
                 {
                     options.RequireHttpsMetadata = false;
                     options.SaveToken = true;

                     options.TokenValidationParameters =
                          new TokenValidationParameters
                          {
                              ValidIssuer = configuration["Tokens:Issuer"],
                              ValidAudience = configuration["Tokens:Audience"],
                              ValidateIssuerSigningKey = true,
                              ValidateLifetime = true,
                              ClockSkew = TimeSpan.Zero,
                              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Tokens:Key"]!))
                          };
                 });

        // Adding Authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        });
        #endregion

        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        }).AddXmlDataContractSerializerFormatters();

        services.Configure<MvcOptions>(options =>
        {
            options.Filters.Add(new AuthorizeFilter());
        });

        //Auto Mapper
        services.AddAutoMapper(typeof(Program));

        // CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        return services;
    }
}
