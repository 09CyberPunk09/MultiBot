using Persistence.Sql.BaseTypes;
using Persistence.Sql.Entites;
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

        public User CreateFromTelegram(string username, long? tgUserId)
        {
            return _userRepository.Add(new()
            {
                Name = username,
                TelegramUserId = tgUserId
            });
        }

        public User Update(User user)
        {
            return _userRepository.Update(user);
        }

        public User GetByTgId(long tgUserId)
        {
            return _userRepository.GetQuery().FirstOrDefault(u => u.TelegramUserId.HasValue && u.TelegramUserId.Value == tgUserId);
        }
    }
}
