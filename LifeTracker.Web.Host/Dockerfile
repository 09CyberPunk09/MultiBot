# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# copy everything else and build app
COPY . ./src/
WORKDIR /src/
RUN dotnet restore
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
ENV ASPNETCORE_ENVIRONMENT=Development
FROM mcr.microsoft.com/dotnet/aspnet:6.0

WORKDIR /app
COPY --from=build /src/LifeTracker.Web.Host/bin/Release/net6.0 ./
EXPOSE 5005
ENTRYPOINT ["dotnet", "LifeTracker.Web.Host.dll"]