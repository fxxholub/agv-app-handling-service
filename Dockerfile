FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Leuze_AGV_Handling_Service.WebAPI/Leuze_AGV_Handling_Service.WebAPI.csproj", "src/Leuze_AGV_Handling_Service.WebAPI/"]
COPY ["src/Leuze_AGV_Handling_Service.Infrastructure/Leuze_AGV_Handling_Service.Infrastructure.csproj", "src/Leuze_AGV_Handling_Service.Infrastructure/"]
COPY ["src/Leuze_AGV_Handling_Service.Core/Leuze_AGV_Handling_Service.Core.csproj", "src/Leuze_AGV_Handling_Service.Core/"]
COPY ["src/Leuze_AGV_Handling_Service.UseCases/Leuze_AGV_Handling_Service.UseCases.csproj", "src/Leuze_AGV_Handling_Service.UseCases/"]
RUN dotnet restore "src/Leuze_AGV_Handling_Service.WebAPI/Leuze_AGV_Handling_Service.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/Leuze_AGV_Handling_Service.WebAPI"
RUN dotnet build "Leuze_AGV_Handling_Service.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Leuze_AGV_Handling_Service.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Leuze_AGV_Handling_Service.WebAPI.dll"]
