using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NationalParksApi.Repositories;
using NationalParksApi.Data;
using Microsoft.EntityFrameworkCore;
using NationalParksApi.Models;
using System.Text.Json;
using System.IO;
using System;
using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Connect DbContext with SQLite
builder.Services.AddDbContext<ParkContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ParkConnection")));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("IdentityConnection")));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    Env.Load();
    var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
    if (string.IsNullOrEmpty(jwtKey))
    {
        throw new Exception("JWT Key is not configured. Set the 'JwtKey' environment variable.");
    }
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddIdentityCore<ApplicationUser>(identityOptions => {
    // optionally configure password settings, lockout, etc.
})
    .AddRoles<IdentityRole>()
    .AddSignInManager()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddScoped<IParkRepository, DbParkRepository>();

builder.Services.AddControllers();

builder.Services.AddHttpClient<IWeatherService, WeatherService>();

var app = builder.Build();

//ensure https
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//Seed Roles
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await RoleSeeder.SeedRolesAndAdminAsync(services);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding roles: {ex.Message}");
    }
}

// Seed DB
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<ParkContext>();

        if (!context.Parks.Any())
        {
            var jsonPath = Path.Combine(AppContext.BaseDirectory, "Data", "parks.json");

            if (!File.Exists(jsonPath))
            {
                throw new FileNotFoundException("parks.json file not found.", jsonPath);
            }

            var json = File.ReadAllText(jsonPath);
            Console.WriteLine(json);

            // seeding seems to only work with the case insensitive set to true
            var parks = JsonSerializer.Deserialize<List<NationalPark>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (parks == null)
            {
                Console.WriteLine("Deserialization returned null");
            }

            if (parks != null && parks.Count > 0)
            {
                context.Parks.AddRange(parks);
                var recordsAdded = context.SaveChanges();
                Console.WriteLine($"{recordsAdded} parks added to the database");
            }
        }
        else
        {
            Console.WriteLine("Database already has parks.");
            Console.WriteLine($"Park count: {context.Parks.Count()}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception occurred during seeding: {ex.Message}");
    }
}

app.MapControllers();

app.Run();
