# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION={$BUILD_CONFIGURATION:-Release}
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["Directory.Packages.props", "."]
COPY ["src/Handling_Service.Core/Handling_Service.Core.csproj", "src/Handling_Service.Core/"]
COPY ["src/Handling_Service.Infrastructure/Handling_Service.Infrastructure.csproj", "src/Handling_Service.Infrastructure/"]
COPY ["src/Handling_Service.Infrastructure.Persistent/Handling_Service.Infrastructure.Persistent.csproj", "src/Handling_Service.Infrastructure.Persistent/"]
COPY ["src/Handling_Service.Infrastructure.Ros2/Handling_Service.Infrastructure.Ros2.csproj", "src/Handling_Service.Infrastructure.Ros2/"]
COPY ["src/Handling_Service.Infrastructure.SignalR/Handling_Service.Infrastructure.SignalR.csproj", "src/Handling_Service.Infrastructure.SignalR/"]
COPY ["src/Handling_Service.UseCases/Handling_Service.UseCases.csproj", "src/Handling_Service.UseCases/"]
COPY ["src/Handling_Service.WebAPI/Handling_Service.WebAPI.csproj", "src/Handling_Service.WebAPI/"]
RUN dotnet restore "src/Handling_Service.WebAPI/Handling_Service.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/Handling_Service.WebAPI"
RUN dotnet build "Handling_Service.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION={$BUILD_CONFIGURATION:-Release}
RUN dotnet publish "Handling_Service.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Startup stage
FROM fxxholub/ros2-dotnet:humble-core-runtime-8.0 AS startup

EXPOSE 8080
EXPOSE 8081

ENV ASPNETCORE_URLS=http://0.0.0.0:8080

RUN apt-get update && apt-get install -y \
    ros-humble-nav2-msgs \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Handling_Service.WebAPI.dll"]