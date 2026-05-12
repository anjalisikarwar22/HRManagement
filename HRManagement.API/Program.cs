using FluentValidation;
using FluentValidation.AspNetCore;
using HRManagement.API.Common;
using HRManagement.API.Data;
using HRManagement.API.DTOs.Departments;
using HRManagement.API.Filters;
using HRManagement.API.Interfaces;
using HRManagement.API.Mappings;
using HRManagement.API.Middleware;
using HRManagement.API.Repositories;
using HRManagement.API.Repository;
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

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelFilter>();
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HRManagement.API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT Token"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

builder.Services.AddDbContext<HRContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOpts =>
        {
            sqlOpts.CommandTimeout(180);
            sqlOpts.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null);
        }));

builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IJobHistoryRepository, JobHistoryRepository>();
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<IJobHistoryService, JobHistoryService>();
builder.Services.AddScoped<IRegionService, RegionService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<JwtService>();

builder.Services.AddAutoMapper(typeof(JobProfile).Assembly);

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<JobDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateRegionDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LocationRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateEmployeeValidator>();
builder.Services.AddScoped<IValidator<CreateDepartmentDto>, CreateDepartmentFluentValidator>();
builder.Services.AddScoped<IValidator<UpdateDepartmentDto>, UpdateDepartmentFluentValidator>();

builder.Services.AddScoped<DepartmentValidator>();
builder.Services.AddScoped<ValidationFilter>();
builder.Services.AddScoped<LogActionFilter>();
builder.Services.AddScoped<DepartmentHeaderFilter>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value!.Errors.Count > 0)
            .Select(x => new
            {
                Field = x.Key,
                Errors = x.Value!.Errors.Select(e => e.ErrorMessage)
            });

        return new BadRequestObjectResult(
            new ApiResponse<object>(false, "Validation Failed", errors));
    };
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                var body = new ApiResponse<object>(false,
                    "Unauthorized. Please log in with a valid token.", null);
                await context.Response.WriteAsync(
                    System.Text.Json.JsonSerializer.Serialize(body));
            },
            OnForbidden = async context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";
                var body = new ApiResponse<object>(false,
                    "Forbidden. You do not have permission to access this resource.", null);
                await context.Response.WriteAsync(
                    System.Text.Json.JsonSerializer.Serialize(body));
            }
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Warm up the DB connection so the first real request isn't slow
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<HRContext>();
        db.Database.OpenConnection();
        db.Database.CloseConnection();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Startup warmup] DB warmup failed: {ex.Message}");
    }
}

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
