using FluentValidation;
using FluentValidation.AspNetCore;
using HRManagement.API.Common;
using HRManagement.API.Data;
using HRManagement.API.DTOs;
using HRManagement.API.Middleware;
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

namespace HRManagement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);






            builder.Services.AddControllers();
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

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
