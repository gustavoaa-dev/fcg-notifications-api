# FCG Notifications API

Microsserviço responsável pelo envio de notificações da plataforma FCG (Fiap Cloud Games).

## Funcionalidades

- Consome `UserCreatedEvent` e envia e-mail de boas-vindas
- Consome `PaymentProcessedEvent` e envia e-mail de confirmação ou falha de compra
- As notificações são simuladas via log no console

## Tecnologias

- .NET 8 (ASP.NET Core Web API)
- MassTransit + RabbitMQ

> Este serviço é puramente orientado a eventos — não possui banco de dados nem endpoints HTTP públicos.

## Como executar

### Pré-requisitos

- .NET SDK 8
- RabbitMQ (local ou container)

### Executar localmente

```bash
dotnet run --project FCG.NotificationsAPI.API
```

### Com Docker

```bash
docker build -t fcg-notifications-api .
docker run -p 5004:8080 fcg-notifications-api
```

### Com Docker Compose

No repositório [fcg-orchestration](https://github.com/gustavoaa-dev/fcg-orchestration), execute:

```bash
docker-compose up -d
```

## Variáveis de ambiente

| Variável | Descrição | Padrão |
|---|---|---|
| `RABBITMQ_HOST` | Host do RabbitMQ | `localhost` |

## Fluxo de eventos

1. **Consome** `UserCreatedEvent` → loga e-mail de boas-vindas
2. **Consome** `PaymentProcessedEvent` → loga e-mail de confirmação (Approved) ou falha (Rejected)
