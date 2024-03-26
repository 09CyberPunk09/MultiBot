FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# copy all sources
WORKDIR /app
COPY . /app/sources
WORKDIR /app/sources
COPY ./Configuration /app/LifeTracker.TelegramBot.IOHandler

# build all projects
RUN dotnet restore
RUN dotnet publish -c Release -o /app/LifeTracker.TelegramBot.IOHandler --no-restore 

#ENTRYPOINT ["LifeTracker.JobExecutor.dll",]
WORKDIR /app/LifeTracker.TelegramBot.IOHandler
CMD ["dotnet", "LifeTracker.TelegramBot.IOHandler.dll"]
#ENTRYPOINT ["tail", "-f", "/dev/null"]