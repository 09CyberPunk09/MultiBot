using Common.Entites;
using Persistence.Common.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services;

public class ReminderAppService : AppService
{
    private readonly IRepository<Reminder> _reminderRepo;
    public ReminderAppService(IRepository<Reminder> reminderRepo)
    {
        _reminderRepo = reminderRepo;
    }

    public List<Reminder> GetAll()
    {
        return _reminderRepo.GetAll().ToList();
    }

    public Reminder Create(Reminder reminder, Guid userId)
    {
        reminder.UserId = userId;
        return _reminderRepo.Add(reminder);
    }

    public Reminder Update(Reminder reminder)
    {
        return _reminderRepo.Update(reminder);
    }

    public Reminder Get(Guid id)
    {
        return _reminderRepo.Get(id);
    }

}
