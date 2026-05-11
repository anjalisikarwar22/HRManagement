using FluentValidation;
using HRManagement.API.Data;
using HRManagement.API.DTOs.Departments;
using HRManagement.API.Filters;
using HRManagement.API.Interfaces;
using HRManagement.API.Middleware;
using HRManagement.API.Repositories;
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

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<HRContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<IValidator<CreateDepartmentDto>, CreateDepartmentFluentValidator>();
            builder.Services.AddScoped<IValidator<UpdateDepartmentDto>, UpdateDepartmentFluentValidator>();
            builder.Services.AddScoped<DepartmentValidator>();
            builder.Services.AddScoped<DepartmentHeaderFilter>();

            var app = builder.Build();

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

