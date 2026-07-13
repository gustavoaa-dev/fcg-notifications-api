FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY FCG.NotificationsAPI.sln .
COPY FCG.NotificationsAPI.API/FCG.NotificationsAPI.API.csproj FCG.NotificationsAPI.API/
COPY FCG.NotificationsAPI.Application/FCG.NotificationsAPI.Application.csproj FCG.NotificationsAPI.Application/
COPY FCG.NotificationsAPI.Domain/FCG.NotificationsAPI.Domain.csproj FCG.NotificationsAPI.Domain/
COPY FCG.NotificationsAPI.Infrastructure/FCG.NotificationsAPI.Infrastructure.csproj FCG.NotificationsAPI.Infrastructure/

RUN dotnet restore

COPY . .

RUN dotnet publish FCG.NotificationsAPI.sln -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "FCG.NotificationsAPI.API.dll"]
