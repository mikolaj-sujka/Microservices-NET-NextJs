
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Data;
using SearchService.Models;

namespace SearchService;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        try
        {
            await DbInitializer.InitializeAsync(app);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        await app.RunAsync();
    }
}