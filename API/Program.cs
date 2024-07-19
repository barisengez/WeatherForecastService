using Business;
using DataAccess.SQL;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Weather Forecast API",
        Description = "An API to input and serve weather forecasts",
        Contact = new OpenApiContact
        {
            Name = "Baris Engez"
        }
    });
});


builder.Services.AddWeatherForecastDataAccess(builder.Configuration);
builder.Services.AddWeatherForecastBusiness();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();
