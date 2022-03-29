using Common.Entites;
using Common.Enums;
using Persistence.Sql;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services
{
    public class TimeTrackingAppService : AppService
    {
        private readonly Repository<TimeTrackingActivity> _activityRepo;
        private readonly Repository<TimeTrackingEntry> _entryRepo;
        private readonly Repository<User> _userRepo;
        public TimeTrackingAppService(
            Repository<TimeTrackingActivity> repo,
            Repository<TimeTrackingEntry> entryRepo,
            Repository<User> userRepo)
        {
            _activityRepo = repo;
            _entryRepo = entryRepo;
            _userRepo = userRepo;
        }

        public TimeTrackingActivity GetActivity(Guid id)
        {
            return _activityRepo.Get(id);
        }

        public void RemoveActivity(Guid id)
        {
            _activityRepo.Remove(_activityRepo.Get(id));
        }

        public void Track(Guid activityId, EntryType type, Guid userId)
        {
            //TODO: Add abiility to set time manually
            var entry = _entryRepo.Add(new()
            {
                EntryType = type,
                UserId = userId,
                ActivityId = activityId,
                TimeStamp = DateTime.Now
            });
            var user = _userRepo.Get(userId);
            user.LastTimeTrackingEntry = entry.Id;
            _userRepo.Update(user);
        }

        public TimeTrackingActivity GetLastTrackedActivity(Guid userId)
        {
            var user = _userRepo.Get(userId);
            var lastEntry = _entryRepo.Get(user.LastTimeTrackingEntry.Value);
            var lastActivity = _activityRepo.Get(lastEntry.ActivityId);
            return lastActivity;
        }

        public TimeTrackingActivity CreateTimeTrackingActivity(string name,Guid userId)
        {
            return _activityRepo.Add(new()
            {
                Name = name,
                UserId = userId
            });
        }

        public List<TimeTrackingActivity> GetAllActivities(Guid userId)
        {
            return _activityRepo
                        .GetTable()
                        .Where(x => x.UserId == userId)
                        .ToList();
        }
    }
}
