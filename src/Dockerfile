# syntax=docker/dockerfile:1.7

ARG DOTNET_VERSION=10.0
ARG BUILD_CONFIGURATION=Release

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
ARG BUILD_CONFIGURATION
WORKDIR /src

COPY EduCrm.sln ./
COPY Shared/ Shared/
COPY Modules/ Modules/

RUN dotnet restore Shared/WebApi/EduCrm.WebApi.csproj

RUN dotnet publish Shared/WebApi/EduCrm.WebApi.csproj \
    -c ${BUILD_CONFIGURATION} \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_GCConserveMemory=5

EXPOSE 8080

COPY --from=build /app/publish ./

USER $APP_UID

ENTRYPOINT ["dotnet", "EduCrm.WebApi.dll"]