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
// Sem default: credencial embutida no codigo e o que a fase proibe.
var rabbitUser = Environment.GetEnvironmentVariable("RABBITMQ_USER")
    ?? throw new InvalidOperationException("A variável RABBITMQ_USER não foi configurada.");
var rabbitPass = Environment.GetEnvironmentVariable("RABBITMQ_PASS")
    ?? throw new InvalidOperationException("A variável RABBITMQ_PASS não foi configurada.");

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserCreatedConsumer>();
    x.AddConsumer<PaymentProcessedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitMqHost, "/", h =>
        {
            h.Username(rabbitUser);
            h.Password(rabbitPass);
        });

        cfg.Message<FCG.Shared.Events.UserCreatedEvent>(m => m.SetEntityName("UserCreatedEvent"));
        cfg.Message<FCG.Shared.Events.PaymentProcessedEvent>(m => m.SetEntityName("PaymentProcessedEvent"));

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddScoped<NotificationService>();

var app = builder.Build();

app.Run();

