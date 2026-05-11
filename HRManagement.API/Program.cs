using FluentValidation;
using FluentValidation.AspNetCore;
using HRManagement.API.Data;
using HRManagement.API.Filters;
using HRManagement.API.Middleware;
using HRManagement.API.Repository;
using HRManagement.API.Repository;
using HRManagement.API.Services;
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