FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RepoVault.CLI/RepoVault.CLI.csproj", "RepoVault.CLI/"]
RUN dotnet restore "RepoVault.CLI/RepoVault.CLI.csproj"
COPY . .
WORKDIR "/src/RepoVault.CLI"
RUN dotnet build "RepoVault.CLI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RepoVault.CLI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RepoVault.CLI.dll"]
