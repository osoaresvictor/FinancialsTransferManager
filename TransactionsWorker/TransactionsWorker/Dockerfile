#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["TransactionsWorker/TransactionsWorker.csproj", "TransactionsWorker/"]
COPY ["TransactionsWorker.Infra/TransactionsWorker.Infra.csproj", "TransactionsWorker.Infra/"]
RUN dotnet restore "TransactionsWorker/TransactionsWorker.csproj"
COPY . .
WORKDIR "/src/TransactionsWorker"
RUN dotnet build "TransactionsWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TransactionsWorker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TransactionsWorker.dll"]