# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy solution file (if exists)
COPY ["OnlineEdu/*.sln", "./"]

# Copy project files - Gerçek proje yapına göre
COPY ["OnlineEdu/OnlineEdu/OnlineEdu.csproj", "OnlineEdu/OnlineEdu/"]
COPY ["OnlineEdu/OnlineEdu.Data/OnlineEdu.Data.csproj", "OnlineEdu/OnlineEdu.Data/"]
COPY ["OnlineEdu/OnlineEdu.DTOs/OnlineEdu.DTOs.csproj", "OnlineEdu/OnlineEdu.DTOs/"]
COPY ["OnlineEdu/OnlineEdu.Entity/OnlineEdu.Entity.csproj", "OnlineEdu/OnlineEdu.Entity/"]

# Restore dependencies
RUN dotnet restore "OnlineEdu/OnlineEdu/OnlineEdu.csproj"

# Copy all source code
COPY . .

# Build the application
WORKDIR "/src/OnlineEdu/OnlineEdu"
RUN dotnet build "OnlineEdu.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "OnlineEdu.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "OnlineEdu.dll"]