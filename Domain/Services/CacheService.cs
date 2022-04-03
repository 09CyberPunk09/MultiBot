using Common.Entites;
using Persistence.Caching.Redis.TelegramCaching;
using Persistence.Sql;
using System;
using System.Linq;

namespace Application.Services
{
    public class CacheService : AppService
    {
        private readonly TelegramCache _cache = new();
        private readonly LifeTrackerRepository<User> _userRepository;

        public CacheService(LifeTrackerRepository<User> userRepo)
        {
            _userRepository = userRepo;
        }

        public void Purge(Guid? userId = null)
        {
            if (userId != null)
            {
                var tgId = _userRepository.GetQuery().FirstOrDefault(x => x.Id == userId).TelegramChatId;
                _cache.PurgeChatData(tgId.Value);
            }
            else
            {
                _cache.PurgeDatabase();
            }
        }
    }
}