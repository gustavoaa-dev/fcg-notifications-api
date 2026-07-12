using FCG.NotificationsAPI.Domain.Events;
using Microsoft.Extensions.Logging;

namespace FCG.NotificationsAPI.Application.Services;

public class NotificationService
{
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(ILogger<NotificationService> logger)
    {
        _logger = logger;
    }

    public void EnviarBoasVindas(UserCreatedEvent evento)
    {
        _logger.LogInformation(
            "[EMAIL ENVIADO] Boas-vindas para {Nome} - {Email}\n" +
            "    Assunto: Bem-vindo à FCG!\n" +
            "    Corpo: Olá {Nome}, seu cadastro foi realizado com sucesso!",
            evento.Nome, evento.Email, evento.Nome);
    }

    public void EnviarConfirmacaoCompra(PaymentProcessedEvent evento)
    {
        if (evento.Status == "Approved")
        {
            _logger.LogInformation(
                "[EMAIL ENVIADO] Confirmação de compra para UserId: {UserId}\n" +
                "    Assunto: Sua compra foi aprovada!\n" +
                "    Corpo: Parabéns! O jogo foi adicionado à sua biblioteca.",
                evento.UserId);
        }
        else
        {
            _logger.LogInformation(
                "[EMAIL ENVIADO] Falha na compra para UserId: {UserId}\n" +
                "    Assunto: Problema com seu pagamento\n" +
                "    Corpo: Infelizmente seu pagamento foi recusado. Tente novamente.",
                evento.UserId);
        }
    }
}
