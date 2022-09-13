using MediatR;
using Microsoft.OpenApi.Models;
using VacationRental.Api;
using VacationRental.Data;
using VacationRental.Domain.Booking;
using VacationRental.Domain.Rental;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(opts => opts.SwaggerDoc("v1", new OpenApiInfo { Title = "Vacation rental information", Version = "v1" }));


builder.Services.AddSingleton<IRentalRepository, RentalRepository>();
builder.Services.AddSingleton<IBookingRepository, BookingRepository>();

var assemblies = new[] { typeof(Startup), typeof(VacationRental.Domain.Startup) };

builder.Services.AddMediatR(assemblies);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();


app.MapControllers();

app.Run();

public partial class Program { }