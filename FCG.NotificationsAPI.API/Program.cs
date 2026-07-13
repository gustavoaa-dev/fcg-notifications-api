using FCG.NotificationsAPI.Application.Consumers;
using FCG.NotificationsAPI.Application.Services;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Information);
builder.Logging.AddFilter("Microsoft", LogLevel.Warning);
builder.Logging.AddFilter("System", LogLevel.Warning);

var rabbitMqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserCreatedConsumer>();
    x.AddConsumer<PaymentProcessedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitMqHost, "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.Message<FCG.Shared.Events.UserCreatedEvent>(m => m.SetEntityName("UserCreatedEvent"));
        cfg.Message<FCG.Shared.Events.PaymentProcessedEvent>(m => m.SetEntityName("PaymentProcessedEvent"));

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddScoped<NotificationService>();

var app = builder.Build();

app.Run();

