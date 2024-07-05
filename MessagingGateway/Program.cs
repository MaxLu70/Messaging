using MassTransit;
using MessageContracts.IMU.Requests;
using MessageGateway.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(c =>
{
    c.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", x =>
        {
            x.Username("guest");
            x.Password("guest");
        });
    });

    var timeout = TimeSpan.FromSeconds(10);
    var serviceAddress = new Uri("rabbitmq://localhost/imu-service");

    c.AddRequestClient<IContribuentiRequest>(serviceAddress, timeout);
});
builder.Services.AddOptions<MassTransitHostOptions>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

