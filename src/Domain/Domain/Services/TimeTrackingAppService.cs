using Common.Entites;
using Common.Models;
using Persistence.Sql;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services
{
    public class TimeTrackingAppService : AppService
    {
        private readonly LifeTrackerRepository<TimeTrackingActivity> _activityRepo;
        private readonly LifeTrackerRepository<TimeTrackingEntry> _entryRepo;
        private readonly LifeTrackerRepository<User> _userRepo;
        public TimeTrackingAppService(
            LifeTrackerRepository<TimeTrackingActivity> repo,
            LifeTrackerRepository<TimeTrackingEntry> entryRepo,
            LifeTrackerRepository<User> userRepo)
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

        public bool TrackOut(Guid activityId,DateTime trackoutTime)
        {
            var entry = _entryRepo.GetTable()
                .Where(x => !x.Completed 
                            && x.ActivityId == activityId)
                .OrderByDescending(x => x.StartTime)
                .FirstOrDefault();
            if (entry == null)
            {
                //todo: create a UserFriendlyException()
                throw new ArgumentException("there is no entry to track out");
                return false;
            }

            entry.Completed = true;
            entry.EndTime = trackoutTime;
            _entryRepo.Update(entry);
            return true;
        }

        public void TrackIn(Guid activityId,DateTime startTime, Guid userId)
        {
            var entry = _entryRepo.Add(new()
            {
                UserId = userId,
                ActivityId = activityId,
                StartTime = startTime,
                //todo: add settings to project. add settings for time how much can be a user idle in time tracking
                EndTime = startTime.AddHours(16),
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

        public TimeTrackingActivity CreateTimeTrackingActivity(string name, Guid userId)
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

        #region Reports
        public List<TimeTrackingReportModel> GenerateReport(DateTime startDate,DateTime endDate,Guid? activityId = null)
        {
            //retrieve all activities
            var entries = _entryRepo.GetTable().Where(x =>  x.StartTime >= startDate && x.EndTime <= endDate);

            //if concrete activity is requested in params,here we add a filter for it
            if(activityId != null)
                entries = entries.Where(x => x.ActivityId == activityId);

            var result = new List<TimeTrackingReportModel>();

            //prefill the result collection with dates and empty timespans
            FillWithDates(result, startDate, endDate);

            //if an entry has started on one date and ended on other - the system keeps the remmant and adds it to the nextr day
            TimeSpan remmant = TimeSpan.Zero;

            for (int i = 0; i < result.Count; i++)
            {
                var date = result[i].Date;
                var entry = entries.FirstOrDefault(x => x.StartTime.Date == date);

                TimeSpan totalTracked = remmant;
                remmant = TimeSpan.Zero;

                //if no entries for that day
                if (entry == null)
                {
                    totalTracked = TimeSpan.Zero;
                }
                //standard case
                else if (entry.StartTime.Date == entry.EndTime.Date)
                {
                    totalTracked += (entry.EndTime - entry.StartTime);
                }
                //not standart situation,when the tracking activity starts in one day and ends in another
                else if (entry.StartTime.Date != entry.EndTime.Date)
                {
                    var diff = entry.EndTime - entry.StartTime;
                    //first,ew calculate the time tracked for current day
                    totalTracked += entry.StartTime.AddDays(1).Date - entry.StartTime;
                    //then, we add the remmant to timespan of another day
                    remmant = entry.EndTime - entry.EndTime.Date;
                }

                result[i].TrackedTime += totalTracked;
            }

            return result;
        }


        private void FillWithDates(List<TimeTrackingReportModel> input,DateTime startDate,DateTime endDate)
        {
            var stepDateFoFill = startDate.Date;
            while (stepDateFoFill != endDate.Date.AddDays(1))
            {
                input.Add(new(stepDateFoFill, TimeSpan.Zero));
                stepDateFoFill = stepDateFoFill.AddDays(1);
            }
        }
        #endregion
    }
}
