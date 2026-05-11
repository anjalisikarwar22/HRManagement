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

            // Controllers + global model-validation filter
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

            // Repositories + Services
            builder.Services.AddScoped<IJobRepository, JobRepository>();
            builder.Services.AddScoped<IJobHistoryRepository, JobHistoryRepository>();
            builder.Services.AddScoped<IJobService, JobService>();
            builder.Services.AddScoped<IJobHistoryService, JobHistoryService>();

            // FluentValidation
            builder.Services.AddValidatorsFromAssemblyContaining<JobDtoValidator>();

            // TODO: Auth will be wired by AuthController teammate.
            // When that's merged, uncomment the [Authorize] attributes in
            // JobsController and JobHistoryController.

            var app = builder.Build();

            // Global exception handler (must be early)
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
