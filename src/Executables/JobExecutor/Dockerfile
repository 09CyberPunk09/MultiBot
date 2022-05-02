FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# copy all sources
WORKDIR /app
COPY . /app/sources
WORKDIR /app/sources

# build all projects
RUN dotnet restore
RUN dotnet publish -c Release -o /app/LifeTracker.JobExecutor --no-restore 

WORKDIR /app/LifeTracker.JobExecutor
CMD ["dotnet", "LifeTracker.JobExecutor.dll"]
#ENTRYPOINT ["tail", "-f", "/dev/null"]