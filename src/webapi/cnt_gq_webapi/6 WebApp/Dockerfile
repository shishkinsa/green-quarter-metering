FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "webapi/cnt_gq_webapi/6 WebApp/GQ.WebApi.WebApp.csproj"
RUN dotnet publish "webapi/cnt_gq_webapi/6 WebApp/GQ.WebApi.WebApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

HEALTHCHECK --interval=10s --timeout=5s --start-period=30s --retries=10 \
  CMD curl -fsS http://localhost:8080/api/v1/health || exit 1

ENTRYPOINT ["dotnet", "GQ.WebApi.WebApp.dll"]
