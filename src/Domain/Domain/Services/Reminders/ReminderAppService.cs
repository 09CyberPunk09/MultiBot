using Application.Services.Reminders.Dto;
using Common.Entites;
using Infrastructure.Queuing;
using Infrastructure.Queuing.Core;
using Microsoft.Extensions.Configuration;
using Persistence.Common.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services.Reminders;

public class ReminderService : AppService
{
    private readonly IRepository<Reminder> _reminderRepo;
    private readonly QueuePublisher _reminderSchedulerPublisher;
    private readonly IRepository<User> _userRepository;


    public ReminderService(IRepository<Reminder> reminderRepo, IConfiguration configuration, IRepository<User> userRepository)
    {
        _reminderRepo = reminderRepo;
        _userRepository = userRepository;
        _reminderSchedulerPublisher =  QueuingHelper.CreatePublisher(configuration["Application:Reminders:ScheduleReminderQueueName"]);
    }

    public List<Reminder> GetAll(Guid? userId = null)
    {
        return userId != null ? _reminderRepo.Where(x => x.UserId == userId.Value).ToList() : _reminderRepo.GetAll().ToList();
    }

    public Reminder Create(CreateReminderDto dto)
    {
        var result = _reminderRepo.Add(new()
        {
            UserId = dto.UserId,
            Name = dto.Text,
            SchedulerExpression = dto.ScheduleExpression.ToJson(),
            IsActive = true
        });
        var user = _userRepository.Get(dto.UserId);
        var chatIds = user.TelegramLogIns.Select(x => x.TelegramChatId).ToArray();

        _reminderSchedulerPublisher.Publish(new SendReminderJobPayload()
        {
            TelegramChatIds = chatIds,
            Text = result.Name,
            ScheduleExpression = dto.ScheduleExpression
        });

        return result;
    }

    public void Delete(Guid reminderId)
    {
        _reminderRepo.Remove(reminderId);
        //TODO: Add queue message to delete a job from jobexecutor
    }

    public void ToggleEnabledDisabledState(Guid reminderId)
    {
        var reminder = _reminderRepo.Get(reminderId);
        reminder.IsActive= !reminder.IsActive;
        _reminderRepo.Update(reminder);
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
