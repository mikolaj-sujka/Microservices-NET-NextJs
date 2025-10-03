
using AuctionService.Data;
using Microsoft.EntityFrameworkCore;

namespace AuctionService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<AuctionDbContext>(opt =>
        {
            opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddControllers();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        try
        {
            DbInitializer.Initialize(app);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        app.MapControllers();

        app.Run();
    }
}