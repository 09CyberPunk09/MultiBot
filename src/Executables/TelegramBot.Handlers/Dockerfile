FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# copy all sources
WORKDIR /app
COPY . /app/sources
WORKDIR /app/sources

# build all projects
RUN dotnet restore
RUN dotnet publish -c Release -o /app/LifeTracker.TelegramBot.Handlers --no-restore 

WORKDIR /app/LifeTracker.TelegramBot.Handlers
CMD ["dotnet", "LifeTracker.TelegramBot.Handlers.dll"]
#ENTRYPOINT ["tail", "-f", "/dev/null"]