﻿using Application.Services;
using Application.TelegramBot.Commands.Pipelines.DefaultBehavior;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.ChatEngine.Commands;

namespace Application.TelegramBot.Pipelines.V2.Pipelines.ToDo.ToDoItems;

[Route("/new_todo", "📋➕ Add ToDo")]
public class CreateToDoCommand : ITelegramCommand
{
    public const string NOTE_TEXT_CACHEKEY = "NoteText";
    public const string CATEGORIES_LIST_CACHEKEY = "CategoriesList";

    public void DefineStages(StageMapBuilder builder)
    {
        builder.Stage<SaveTextAndAskCategory>();
        builder.Stage<AcceptCategoryAndSave>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var todoTextFromUnrecognizedCommand = ctx.Cache.Get<string>(SuggestActionsCommand.TEXTCONTENT_CACHEKEY);
        if (todoTextFromUnrecognizedCommand != null)
        {
            ctx.Response.InvokeNextImmediately = true;
            ctx.Message.Text = todoTextFromUnrecognizedCommand;
            return ContentResponse.New(null);
        }

        return ContentResponse.Text("Enter note text:");
    }
}

public class SaveTextAndAskCategory : ITelegramStage
{
    private readonly ToDoAppService _todoService;
    public SaveTextAndAskCategory(ToDoAppService toDoAppService)
    {
        _todoService = toDoAppService;
    }
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        string noteText = ctx.Message.Text;
        var todoTextFromUnrecognizedCommand = ctx.Cache.Get<string>(SuggestActionsCommand.TEXTCONTENT_CACHEKEY, true);
        if (todoTextFromUnrecognizedCommand != null)
        {
            noteText = todoTextFromUnrecognizedCommand;
        }

        ctx.Cache.Set(CreateToDoCommand.NOTE_TEXT_CACHEKEY, noteText);

        var categories = _todoService.GetAllCategories(ctx.User.Id);
        var b = new StringBuilder();
        b.AppendLine("Your Categories: ");

        int counter = 0;
        var dictionary = new Dictionary<int, Guid>();

        foreach (var item in categories)
        {
            ++counter;
            b.AppendLine($"🔷 {counter}. {item.Name}");
            dictionary[counter] = item.Id;
        }

        ctx.Cache.Set(CreateToDoCommand.CATEGORIES_LIST_CACHEKEY, dictionary);

        return ContentResponse.New(new()
        {
            Text = b.ToString(),
            MultiMessages = new()
            {
                new()
                {
                    Text = "Enter a number near the category of the ToDo:"
                }
            }
        });
    }
}

public class AcceptCategoryAndSave : ITelegramStage
{
    private readonly ToDoAppService _todoService;
    public AcceptCategoryAndSave(ToDoAppService toDoAppService)
    {
        _todoService = toDoAppService;
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var dict = ctx.Cache.Get<Dictionary<int, Guid>>(CreateToDoCommand.CATEGORIES_LIST_CACHEKEY);
        if (!(int.TryParse(ctx.Message.Text, out var number) && number >= 0 && number <= dict.Count))
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.Text("⛔️ Enter a number form the suggested list");
        }

        var categoryId = dict[number];
        var todoItemText = ctx.Cache.Get<string>(CreateToDoCommand.NOTE_TEXT_CACHEKEY);
        _todoService.CreateItem(ctx.User.Id, categoryId, todoItemText);

        return ContentResponse.Text("✅ Done.");
    }
}