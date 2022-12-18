namespace Persistence.Caching.Redis
{
    public enum DatabaseType
    {
        Pipelines = 0,
        System = 1,
        Telegram,
        TelegramUserCache,
        ApplicationAccessibility,
        WebApp,
        TelegramLogins
    }
}