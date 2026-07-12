using FCG.NotificationsAPI.Application.Services;
using FCG.NotificationsAPI.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FCG.NotificationsAPI.Application.Consumers;

public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
    private readonly NotificationService _notificationService;
    private readonly ILogger<UserCreatedConsumer> _logger;

    public UserCreatedConsumer(NotificationService notificationService, ILogger<UserCreatedConsumer> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    public Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        var evento = context.Message;

        _logger.LogInformation(
            "Evento UserCreated recebido - UserId: {UserId}, Email: {Email}",
            evento.UserId, evento.Email);

        _notificationService.EnviarBoasVindas(evento);

        return Task.CompletedTask;
    }
}
