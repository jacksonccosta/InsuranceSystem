# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/InsuranceSystem.API/InsuranceSystem.API.csproj", "src/InsuranceSystem.API/"]
COPY ["src/InsuranceSystem.Application/InsuranceSystem.Application.csproj", "src/InsuranceSystem.Application/"]
COPY ["src/InsuranceSystem.Domain/InsuranceSystem.Domain.csproj", "src/InsuranceSystem.Domain/"]
COPY ["src/InsuranceSystem.Infrastructure/InsuranceSystem.Infrastructure.csproj", "src/InsuranceSystem.Infrastructure/"]
RUN dotnet restore "src/InsuranceSystem.API/InsuranceSystem.API.csproj"
COPY . .
WORKDIR "/src/src/InsuranceSystem.API"
RUN dotnet build "InsuranceSystem.API.csproj" -c Release -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish "InsuranceSystem.API.csproj" -c Release -o /app/publish

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InsuranceSystem.API.dll"]