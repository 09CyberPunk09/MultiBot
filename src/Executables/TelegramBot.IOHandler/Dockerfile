FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# copy all sources
WORKDIR /app
COPY . /app/sources
# COPY ./appSettings.json /app/LifeTracker.TelegramBot.IOHandler
# COPY ./nlog.config /app/LifeTracker.TelegramBot.IOHandler
WORKDIR /app/sources

# build all projects
RUN dotnet restore
RUN dotnet publish -c Release -o /app/LifeTracker.TelegramBot.IOHandler --no-restore 


#ENTRYPOINT ["LifeTracker.JobExecutor.dll",]
WORKDIR /app/LifeTracker.TelegramBot.IOHandler
CMD ["dotnet", "LifeTracker.TelegramBot.IOHandler.dll"]
#ENTRYPOINT ["tail", "-f", "/dev/null"]