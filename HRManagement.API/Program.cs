using FluentValidation;
using FluentValidation.AspNetCore;
using HRManagement.API.Common;
using HRManagement.API.Data;
using HRManagement.API.DTOs;
using HRManagement.API.Middleware;
using HRManagement.API.Repository;
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
using HRManagement.API.Validators.Employee;
using HRManagement.API.Validators.Location;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

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






            builder.Services.AddControllers();
            // Controllers + global model-validation filter
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ValidateModelFilter>();
            });

            builder.Services.AddEndpointsApiExplorer();


            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "HRManagement.API",
                        Version = "v1"
                    });

                options.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description =
                            "Enter JWT Token"
                    });

                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
            {
                new OpenApiSecurityScheme
                {
                    Reference =
                        new OpenApiReference
                        {
                            Type =
                                ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                },

                Array.Empty<string>()
            }
                    });
            });




            // DbContext
            builder.Services.AddDbContext<HRContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                ));

            builder.Services.AddScoped<ILocationRepository, LocationRepository>();
            
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();



            builder.Services.AddScoped<ILocationService, LocationService>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();

            builder.Services.AddScoped<IAuthService,AuthService>();

            builder.Services.AddScoped<JwtService>();

            builder.Services.AddFluentValidationAutoValidation();

            builder.Services.AddValidatorsFromAssemblyContaining<CreateLocationValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<CreateEmployeeValidator>();

            // Custom Validation Response
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(x => x.Value!.Errors.Count > 0)
                        .Select(x => new
                        {
                            Field = x.Key,
                            Errors = x.Value!.Errors
                                .Select(e => e.ErrorMessage)
                        });

                    return new BadRequestObjectResult(
                        new ApiResponse<object>(
                            false,
                            "Validation Failed",
                            errors
                        )
                    );
                };
            });


            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        IssuerSigningKey =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                                              builder.Configuration["Jwt:Key"]))
                    };
            });

            builder.Services.AddAuthorization();
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
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}

