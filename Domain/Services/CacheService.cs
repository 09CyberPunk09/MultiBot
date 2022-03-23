using Persistence.Caching.Redis.TelegramCaching;

using System;
using System.Linq;
using Persistence.Sql;
using Common.Entites;

namespace Application.Services
{
    public class CacheService : AppService
    {
        private readonly TelegramCache _cache = new();
        private readonly Repository<User> _userRepository;
        public CacheService(Repository<User> userRepo)
        {
            _userRepository = userRepo;
        }
        public void Purge(Guid? userId = null)
        {
            if (userId != null)
            {
                var tgId = _userRepository.GetQuery().FirstOrDefault(x => x.Id == userId).TelegramUserId;
                _cache.PurgeChatData(tgId.Value);
            }
            else
            {
                _cache.PurgeDatabase();
            }
        }
    }
}
