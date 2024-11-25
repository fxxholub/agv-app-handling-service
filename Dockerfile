# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["Directory.Packages.props", "."]
COPY ["src/Leuze_AGV_Handling_Service.Core/Leuze_AGV_Handling_Service.Core.csproj", "src/Leuze_AGV_Handling_Service.Core/"]
COPY ["src/Leuze_AGV_Handling_Service.Infrastructure/Leuze_AGV_Handling_Service.Infrastructure.csproj", "src/Leuze_AGV_Handling_Service.Infrastructure/"]
COPY ["src/Leuze_AGV_Handling_Service.Infrastructure.Persistent/Leuze_AGV_Handling_Service.Infrastructure.Persistent.csproj", "src/Leuze_AGV_Handling_Service.Infrastructure.Persistent/"]
COPY ["src/Leuze_AGV_Handling_Service.Infrastructure.Ros2/Leuze_AGV_Handling_Service.Infrastructure.Ros2.csproj", "src/Leuze_AGV_Handling_Service.Infrastructure.Ros2/"]
COPY ["src/Leuze_AGV_Handling_Service.Infrastructure.SignalR/Leuze_AGV_Handling_Service.Infrastructure.SignalR.csproj", "src/Leuze_AGV_Handling_Service.Infrastructure.SignalR/"]
COPY ["src/Leuze_AGV_Handling_Service.UseCases/Leuze_AGV_Handling_Service.UseCases.csproj", "src/Leuze_AGV_Handling_Service.UseCases/"]
COPY ["src/Leuze_AGV_Handling_Service.WebAPI/Leuze_AGV_Handling_Service.WebAPI.csproj", "src/Leuze_AGV_Handling_Service.WebAPI/"]
RUN dotnet restore "src/Leuze_AGV_Handling_Service.WebAPI/Leuze_AGV_Handling_Service.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/Leuze_AGV_Handling_Service.WebAPI"
RUN dotnet build "Leuze_AGV_Handling_Service.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "Leuze_AGV_Handling_Service.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Startup stage
FROM fxxholub/ros2-dotnet:humble-core-runtime-8.0 AS startup

EXPOSE 8080
EXPOSE 8081

WORKDIR /app

COPY --from=publish /app/publish .
COPY ./tests/ProcessScripts ./ProcessScripts

ENTRYPOINT ["dotnet", "Leuze_AGV_Handling_Service.WebAPI.dll"]