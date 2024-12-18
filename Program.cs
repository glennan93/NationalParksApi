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

var builder = WebApplication.CreateBuilder(args);

// Connect DbContext with SQLite
builder.Services.AddDbContext<ParkContext>(options =>
    options.UseSqlite("Data Source=parks.db"));

builder.Services.AddScoped<IParkRepository, DbParkRepository>();

builder.Services.AddControllers();

var app = builder.Build();

// Seed
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
