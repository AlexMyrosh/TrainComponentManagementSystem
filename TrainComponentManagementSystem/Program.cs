using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TrainComponentManagementSystem.AutoMapper;
using TrainComponentManagementSystem.Context;
using TrainComponentManagementSystem.Middlewares;
using TrainComponentManagementSystem.Services;
using TrainComponentManagementSystem.Services.Interfaces;

namespace TrainComponentManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            builder.Host.UseSerilog();

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddMemoryCache();

            builder.Services.AddValidatorsFromAssemblyContaining<Program>();
            builder.Services.AddFluentValidationAutoValidation();

            builder.Services.AddAutoMapper(cfg => {
                cfg.AddProfile<MappingProfile>();
            });

            builder.Services.AddSingleton<ICacheService, MemoryCacheService>();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            var app = builder.Build();
            app.UseSerilogRequestLogging();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
