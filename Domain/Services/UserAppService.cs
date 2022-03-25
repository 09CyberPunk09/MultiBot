using Common.Entites;
using Persistence.Sql;
using System.Linq;

namespace Application.Services
{
    public class UserAppService : AppService
    {
        private readonly Repository<User> _userRepository;

        public UserAppService(Repository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public User CreateFromTelegram(string username, long tgChatId)
        {
            return _userRepository.Add(new()
            {
                Name = username,
                TelegramChatId = tgChatId
            });
        }

        public User Update(User user)
        {
            return _userRepository.Update(user);
        }

        public User GetByTgId(long tgUserId)
        {
            return _userRepository.GetQuery().FirstOrDefault(u => u.TelegramChatId.HasValue && u.TelegramChatId.Value == tgUserId);
        }
    }
}