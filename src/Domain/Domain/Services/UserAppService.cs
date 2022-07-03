using Common.Entites;
using Persistence.Master;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services
{
    public class UserAppService : AppService
    {
        private readonly LifeTrackerRepository<User> _userRepository;
        private readonly TagAppService _tagService;

        public UserAppService(LifeTrackerRepository<User> userRepository,
            TagAppService tagService)
        {
            _userRepository = userRepository;
            _tagService = tagService;
        }

        public List<User> GetAll()
        {
            return _userRepository.GetAll().ToList();
        }

        public User CreateFromTelegram(string username, long tgChatId)
        {
            var user = _userRepository.Add(new()
            {
                Name = username,
                TelegramChatId = tgChatId
            });
            InitializeUserEntities(user.Id);
            return user;
        }

        public User Update(User user)
        {
            return _userRepository.Update(user);
        }

        public User GetByTgId(long tgUserId)
        {
            return _userRepository.GetQuery().FirstOrDefault(u => u.TelegramChatId.HasValue && u.TelegramChatId.Value == tgUserId);
        }

        public void InitializeUserEntities(Guid userId)
        {
            //TODO: Add here some initialization code in future
        }

        public User GetUser(string emailAddress)
        {
            return _userRepository.GetQuery().FirstOrDefault(x => x.EmailAddress == emailAddress);
        }

        public void SignUp(string name, string email, string password)
        {
            var existingUser = _userRepository.FirstOrDefault(x => x.EmailAddress == email);

            if (existingUser != null)
                throw new Exception("User with this email address already exists!");
            else
            {
                var newUser = new User()
                {
                    EmailAddress = email,
                    Password = password,
                    Name = name,
                    TelegramLoggedIn = false
                };

                _userRepository.Add(newUser);
            }
        }
    }
}