using Application.TelegramBot.Pipelines.Old.MessagePipelines.Scheduling;
using Common.Entites.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.ChatEngine.Commands;
using TelegramBot.ChatEngine.Commands.Repsonses;
using TelegramBot.ChatEngine.Commands.Routing;
using static Application.TelegramBot.Commands.Pipelines.SChedulingV2.Pipelines.SelectDayStage;
using static TelegramBot.ChatEngine.Commands.Repsonses.Menu;

namespace Application.TelegramBot.Commands.Pipelines.SChedulingV2.Pipelines;

//[Route("/every_days_at")]
public class EveryDaySAtCommand : ITelegramStage
{
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        ctx.Response.InvokeNextImmediately = true;
        ctx.Cache.Remove(SELECTEDDAYS_CACHEKEY);
        return Task.FromResult(new StageResult()
        {
            Content = new()
            {
                Text = "Select the days:"
            },
            NextStage = typeof(SelectDayStage).FullName
        });

    }
}

public class SelectDayStage : ITelegramStage
{
    private readonly RoutingTable _routingTable;
    public SelectDayStage(RoutingTable routingTable)
    {
        _routingTable = routingTable;
    }

    public const string SELECTEDDAYS_CACHEKEY = "SelectedDays";

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        //if the message is empty - we invoked immediately the next stage after command. The selection just started and there is nothing to parse yet
        if (ctx.Message.Text == string.Empty)
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.New(
                new()
                {
                    Text = "PLease, select an item from menu",
                    Menu = new(MenuType.MessageMenu, DaysMenu.Select(x => new[] { new Button(x.DisplayName, ((int)x.Action).ToString()) }))
                });
        }

        var userSelectedDays = ctx.Cache.Get<List<DaysMenuAction>>(SELECTEDDAYS_CACHEKEY);
        userSelectedDays ??= new List<DaysMenuAction>();

        if (!Enum.TryParse<DaysMenuAction>(ctx.Message.Text, out var selection))
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.Text("PLease, select an item from menu");
        }

        switch (selection)
        {
            case DaysMenuAction.Monday:
            case DaysMenuAction.Tuesday:
            case DaysMenuAction.Wednesday:
            case DaysMenuAction.Thursday:
            case DaysMenuAction.Friday:
            case DaysMenuAction.Saturday:
            case DaysMenuAction.Sunday:
                ctx.Response.ForbidNextStageInvokation();

                var selectedMenuItem = DaysMenu.First(x => x.Action == selection);
                userSelectedDays.Add(selectedMenuItem.Action);

                var menuUtems = DaysMenu.Where(x => !userSelectedDays.Any(y => x.Action == y));

                ctx.Cache.Set(SELECTEDDAYS_CACHEKEY, userSelectedDays);

                var sb = new StringBuilder();
                userSelectedDays.ForEach(x =>
                {
                    var found = DaysMenu.FirstOrDefault(y => y.Action == x);
                    if (found != null)
                    {
                        sb.AppendLine(found.DisplayName);
                    }
                });

                menuUtems = menuUtems.Concat(ActionsMenu);

                return ContentResponse.New(new()
                {
                    Text = userSelectedDays.Count == 0 ? @$"No selected days yet. Select the days from menu" :
                    $"Selected days: {Environment.NewLine} {sb}",
                    Menu = new(MenuType.MessageMenu, menuUtems.Select(x => new[] { new Button(x.DisplayName, ((int)x.Action).ToString()) })),
                    Edited = true,
                });
            case DaysMenuAction.Confirm:
                return Task.FromResult(new StageResult()
                {
                    Content = new()
                    {
                        Text = "Next, enter time in format HH:MM, HH:MM,..."
                    },
                    NextStage = typeof(AcceptAsledTimesStage).FullName
                });
            case DaysMenuAction.CancelLastDesicion:
                ctx.Response.ForbidNextStageInvokation();
                var last = userSelectedDays.LastOrDefault();
                if (last != null)
                {
                    userSelectedDays.Remove(last);

                    var menuUtems1 = DaysMenu.Where(x => !userSelectedDays.Any(y => x.Action == y));

                    ctx.Cache.Set(SELECTEDDAYS_CACHEKEY, userSelectedDays);

                    var sb1 = new StringBuilder();
                    userSelectedDays.ForEach(x =>
                    {
                        var found = DaysMenu.FirstOrDefault(y => y.Action == x);
                        if (found != null)
                        {
                            sb1.AppendLine(found.DisplayName);
                        }
                    });

                    menuUtems1 = menuUtems1.Concat(ActionsMenu);

                    return ContentResponse.New(new()
                    {
                        Text = userSelectedDays.Count == 0 ? @$"No selected days yet. Select the days from menu" :
                        $"Selected days: {Environment.NewLine} {sb1}",
                        Menu = new(MenuType.MessageMenu, menuUtems1.Select(x => new[] { new Button(x.DisplayName, ((int)x.Action).ToString()) })),
                        Edited = true,
                    });

                }
                break;
            default:
                break;
        }
        return null;
    }

    //menu template
    private readonly List<DaysMenuItem> DaysMenu = new()
    {
        new DaysMenuItem(DaysMenuAction.Monday, "📅 Monday"),
        new DaysMenuItem(DaysMenuAction.Tuesday, "📅 Tuesday"),
        new DaysMenuItem(DaysMenuAction.Wednesday, "📅Wednesday"),
        new DaysMenuItem(DaysMenuAction.Thursday, "📅 Thursday"),
        new DaysMenuItem(DaysMenuAction.Friday, "📅 Friday"),
        new DaysMenuItem(DaysMenuAction.Saturday, "📅Saturday"),
        new DaysMenuItem(DaysMenuAction.Sunday, "📅 Sunday"),
    };
    private readonly List<DaysMenuItem> ActionsMenu = new()
    {
        new DaysMenuItem(DaysMenuAction.Confirm, "➡️ Confirm"),
        new DaysMenuItem(DaysMenuAction.CancelLastDesicion, "❌ Cancel"),
    };

    public enum DaysMenuAction
    {
        Monday = -144386,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday,
        Confirm,
        CancelLastDesicion
    }
    class DaysMenuItem
    {
        public DaysMenuItem(DaysMenuAction action, string displayName)
        {
            Action = action;
            DisplayName = displayName;
        }
        public DaysMenuAction Action { get; set; }
        public string DisplayName { get; set; }
    }
}

public class AcceptAsledTimesStage : ITelegramStage
{
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var text = ctx.Message.Text;
        try
        {
            var result = TimeParser.Parse(text);
            //0 0,35 7 ? * * *
            //ScheduleExpressionDto
            var crons = new List<string>();
            var selectedDays = ctx.Cache.Get<List<DaysMenuAction>>(SELECTEDDAYS_CACHEKEY, true);
            var selectedDaysCodes = selectedDays.Select(x => Enum.GetName(x).Substring(0, 3).ToUpper());
            StringBuilder selectedTimes = new();
            var daysString = string.Join(",", selectedDaysCodes);
            foreach (var tuple in result)
            {
                var cron = $"0 0,{tuple.Item2} {tuple.Item1} ? * {daysString} *";
                selectedTimes.Append($" {tuple.Item1}:{tuple.Item2} ");
                crons.Add(cron);
            }

            var schedulerConfig = new ScheduleExpressionDto(crons);
            schedulerConfig.Description = $"Every {string.Join(',', selectedDays.Select(x => Enum.GetName<DaysMenuAction>(x)))} on {selectedTimes.ToString()}";
            ctx.Cache.Set(ScheduleExpressionDto.CACHEKEY, schedulerConfig);

            ctx.Response.InvokeNextImmediately = true;
            return Task.FromResult(new StageResult() { });
        }
        catch (ArgumentException)
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.Text("You entered a not valid value. Try entering a string in format HH:MM, HH:MM,...");
        }
    }
}
