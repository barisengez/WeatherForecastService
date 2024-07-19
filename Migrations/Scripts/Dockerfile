# Use the official .NET SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy the csproj and restore as distinct layers
COPY ../DataAccess.SQL/DataAccess.SQL.csproj ./DataAccess.SQL/
COPY ../Migrations/Migrations.csproj ./Migrations/

RUN dotnet restore "Migrations/Migrations.csproj"

# Copy the rest of the application and build it
COPY . ./
WORKDIR "Migrations"
RUN dotnet publish "Migrations.csproj" -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/Migrations/out .

# Set the entry point
ENTRYPOINT ["dotnet", "Migrations.dll"]