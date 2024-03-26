using Common.Entites;
using Common.Enums;
using Persistence.Caching.Redis;
using Persistence.Common.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services.Users;

public class UserAppService : AppService
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<UserFeatureFlag> _userFeatureFlagRepository;
    private readonly IRepository<TelegramLogIn> _telegramLoginRepository;
    public UserAppService(
        IRepository<User> userRepository,
        IRepository<UserFeatureFlag> userFeatureFlagRepository,
        IRepository<TelegramLogIn> telegramLoginRepository)
    {
        _userRepository = userRepository;
        _userFeatureFlagRepository = userFeatureFlagRepository;
        _telegramLoginRepository = telegramLoginRepository;
    }

    public void TelegramLogOut(long telegramUserId)
    {
        var telegramLogin = _telegramLoginRepository.FirstOrDefault(x => x.TelegramUserId == telegramUserId);
        if(telegramLogin != null)
            _telegramLoginRepository.RemovePhysically(telegramLogin.Id);
    }

    public bool TelegramLogin(string emailAddress, string password, long userId, long chatId)
    {
        var user = GetUserByEmail(emailAddress);
        if (user == null)
            throw new Exception($"There is no user with email {emailAddress} in the system");
        if (user.Password != password)
            throw new Exception($"Invalid Password");
        //todo: add telegramlogin to db
        //add repo for it
        //add adding here
        //if you are in mood - move all telegram-dependent logics awaur from here to a separate service
        var login = _telegramLoginRepository.Add(new TelegramLogIn()
        {
            TelegramUserId = userId,
            UserId = user.Id,
            TelegramChatId = chatId
        });

        user.TelegramLogIns.Add(login);

        _userRepository.Update(user);
        return true;
    }

    public List<User> GetAll()
    {
        return _userRepository.GetAll().ToList();
    }

    public User Update(User user, long? telegramUserId = null)
    {
        return _userRepository.Update(user);
    }

    public User GetByTgId(long tgUserId)
    {
        User user;
        var userId = _telegramLoginRepository.FirstOrDefault(x => x.TelegramUserId == tgUserId).UserId;
        user = _userRepository.Get(userId);
        return user;
    }

    public async Task InitializeUserEntities(Guid userId)
    {
        //TODO: Add here some initialization code in future
    }

    public User GetUserByEmail(string emailAddress)
    {
        return _userRepository.FirstOrDefault(x => x.EmailAddress == emailAddress);
    }

    public async Task<Guid> SignUp(string name, string email, string password)
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
                Name = name
            };
            var newUserId = _userRepository.Add(newUser).Id;
            _userFeatureFlagRepository.Add(new()
            {
                FeatureFlag = FeatureFlag.BasicFunctionality,
                UserId = newUserId
            });
            await InitializeUserEntities(newUserId);
            return newUserId;
        }
    }
}