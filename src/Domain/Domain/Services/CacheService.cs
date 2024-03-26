using Common.Entites;
using Common.Infrastructure;
using Persistence.Caching.Redis;
using Persistence.Caching.Redis.TelegramCaching;
using Persistence.Common.DataAccess.Interfaces;
using Persistence.Master;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services;

public class CacheService : AppService
{
    private readonly TelegramCache _telegramCache = new();
    private readonly IRepository<User> _userRepository;

    public CacheService(IRepository<User> userRepo)
    {
        _userRepository = userRepo;
    }

    public List<ApplicationAccessibilityData> GetIdleApplications()
    {
        var cache = new Cache(DatabaseType.ApplicationAccessibility);
        var keys = cache.GetAllkeys();
        var values = keys.Select(key => cache.Get<ApplicationAccessibilityData>(key));
        return values.ToList();
    }

    public void Purge(Guid? userId = null)
    {
        if (userId != null)
        {
            var tgId = _userRepository.FirstOrDefault(x => x.Id == userId).TelegramChatId;
            _telegramCache.PurgeChatData(tgId.Value);
        }
        else
        {
            _telegramCache.PurgeDatabase();
        }
    }
}