FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# copy all sources
WORKDIR /app
COPY . /app/sources
WORKDIR /app/sources
COPY ./Configuration /app/LifeTracker.Migrator

# build all projects
RUN dotnet restore
RUN dotnet publish -c Release -o /app/LifeTracker.Migrator --no-restore 

WORKDIR /app/LifeTracker.Migrator
CMD ["dotnet", "LifeTracker.Migrator.dll"]
#ENTRYPOINT ["tail", "-f", "/dev/null"]