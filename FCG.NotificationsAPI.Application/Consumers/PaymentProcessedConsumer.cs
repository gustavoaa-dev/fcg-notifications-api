using FCG.NotificationsAPI.Application.Services;
using FCG.Shared.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FCG.NotificationsAPI.Application.Consumers;

public class PaymentProcessedConsumer : IConsumer<PaymentProcessedEvent>
{
    private readonly NotificationService _notificationService;
    private readonly ILogger<PaymentProcessedConsumer> _logger;

    public PaymentProcessedConsumer(NotificationService notificationService, ILogger<PaymentProcessedConsumer> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    public Task Consume(ConsumeContext<PaymentProcessedEvent> context)
    {
        var evento = context.Message;

        _logger.LogInformation(
            "Evento PaymentProcessed recebido - OrderId: {OrderId}, Status: {Status}",
            evento.OrderId, evento.Status);

        _notificationService.EnviarConfirmacaoCompra(evento);

        return Task.CompletedTask;
    }
}
