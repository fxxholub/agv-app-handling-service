# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy-arm64v8 AS build
ARG BUILD_CONFIGURATION=${BUILD_CONFIGURATION}
ARG TARGET_RUNTIME=linux-arm64
WORKDIR /src
COPY . .
RUN dotnet restore "src/Leuze_AGV_Handling_Service.WebAPI/Leuze_AGV_Handling_Service.WebAPI.csproj" -r $TARGET_RUNTIME
WORKDIR "/src/src/Leuze_AGV_Handling_Service.WebAPI"
RUN dotnet publish "Leuze_AGV_Handling_Service.WebAPI.csproj" -c $BUILD_CONFIGURATION -r $TARGET_RUNTIME --self-contained -o /app

# Startup stage
FROM arm64v8/ros:humble-ros-base-jammy AS startup
USER $APP_UID
EXPOSE 8080
EXPOSE 8081
ARG PROCESS_SCRIPTS_PATH=${PROCESS_SCRIPTS_PATH}
ENV PROCESS_SCRIPTS_PATH_INNER=ProcessScripts
WORKDIR /app
COPY --from=build /app .
COPY $PROCESS_SCRIPTS_PATH /app/${PROCESS_SCRIPTS_PATH_INNER}
CMD ./Leuze_AGV_Handling_Service.WebAPI
