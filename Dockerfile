# Base stage
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
FROM ros:humble-ros-core AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=${BUILD_CONFIGURATION}
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["Directory.Packages.props", "."]
COPY ["src/Leuze_AGV_Handling_Service.WebAPI/Leuze_AGV_Handling_Service.WebAPI.csproj", "src/Leuze_AGV_Handling_Service.WebAPI/"]
COPY ["src/Leuze_AGV_Handling_Service.UseCases/Leuze_AGV_Handling_Service.UseCases.csproj", "src/Leuze_AGV_Handling_Service.UseCases/"]
COPY ["src/Leuze_AGV_Handling_Service.Core/Leuze_AGV_Handling_Service.Core.csproj", "src/Leuze_AGV_Handling_Service.Core/"]
COPY ["src/Leuze_AGV_Handling_Service.Infrastructure/Leuze_AGV_Handling_Service.Infrastructure.csproj", "src/Leuze_AGV_Handling_Service.Infrastructure/"]
COPY ["src/Leuze_AGV_Handling_Service.Infrastructure.Persistent/Leuze_AGV_Handling_Service.Infrastructure.Persistent.csproj", "src/Leuze_AGV_Handling_Service.Infrastructure.Persistent/"]
RUN dotnet restore "src/Leuze_AGV_Handling_Service.WebAPI/Leuze_AGV_Handling_Service.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/Leuze_AGV_Handling_Service.WebAPI"
RUN dotnet build "Leuze_AGV_Handling_Service.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=${BUILD_CONFIGURATION}
RUN dotnet publish "Leuze_AGV_Handling_Service.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ProcessScripts /app/ProcessScripts
#CMD ["dotnet", "Leuze_AGV_Handling_Service.WebAPI.dll"]
CMD . /opt/ros/humble/setup.sh && dotnet Leuze_AGV_Handling_Service.WebAPI.dll