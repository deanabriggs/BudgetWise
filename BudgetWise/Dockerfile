# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY BudgetWise.csproj .
RUN dotnet restore BudgetWise.csproj

# Copy everything else and build
COPY . .
RUN dotnet build BudgetWise.csproj -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish BudgetWise.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "BudgetWise.dll"]
