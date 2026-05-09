using FluentValidation;
using HRManagement.API.Data;
using HRManagement.API.Filters;
using HRManagement.API.Middleware;
using HRManagement.API.Repository;
using HRManagement.API.Services;
using HRManagement.API.Validators;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Register controllers with global ValidateModelFilter
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ValidateModelFilter>();
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // DbContext
            builder.Services.AddDbContext<HRContext>(opt =>
                opt.UseSqlServer(
                    builder.Configuration.GetConnectionString("HR")
                    ?? "Server=.\\sqlexpress;Database=HR;Trusted_Connection=True;TrustServerCertificate=True;"));

            // Repositories
            builder.Services.AddScoped<IJobRepository, JobRepository>();
            builder.Services.AddScoped<IJobHistoryRepository, JobHistoryRepository>();

            // Services
            builder.Services.AddScoped<IJobService, JobService>();
            builder.Services.AddScoped<IJobHistoryService, JobHistoryService>();

            // FluentValidation - auto register all validators
            builder.Services.AddValidatorsFromAssemblyContaining<JobDtoValidator>();

            var app = builder.Build();

            // Global exception handling
            app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
