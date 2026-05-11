using FluentValidation;
using FluentValidation.AspNetCore;
using HRManagement.API.Data;
using HRManagement.API.Filters;
using HRManagement.API.Middleware;
using HRManagement.API.Repository;
using HRManagement.API.Repository;
using HRManagement.API.Services;
using HRManagement.API.Data;
using HRManagement.API.DTOs.Departments;
using HRManagement.API.Filters;
using HRManagement.API.Interfaces;
using HRManagement.API.Middleware;
using HRManagement.API.Repositories;
using HRManagement.API.Services;
using HRManagement.API.Validators;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HRContext>(options =>
    options.UseSqlServer(
        builder.Configuration
            .GetConnectionString("HRConnection")));

builder.Services.AddScoped<IRegionRepository,
    RegionRepository>();
builder.Services.AddScoped<ICountryRepository,
    CountryRepository>();

builder.Services.AddScoped<IRegionService,
    RegionService>();
builder.Services.AddScoped<ICountryService,
    CountryService>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining
    <CreateRegionDtoValidator>();

builder.Services.AddScoped<ValidationFilter>();
builder.Services.AddScoped<LogActionFilter>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
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
            builder.Services.AddDbContext<HRContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<IValidator<CreateDepartmentDto>, CreateDepartmentFluentValidator>();
            builder.Services.AddScoped<IValidator<UpdateDepartmentDto>, UpdateDepartmentFluentValidator>();
            builder.Services.AddScoped<DepartmentValidator>();
            builder.Services.AddScoped<DepartmentHeaderFilter>();

            var app = builder.Build();

            // Global exception handler (must be early)
            app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}

