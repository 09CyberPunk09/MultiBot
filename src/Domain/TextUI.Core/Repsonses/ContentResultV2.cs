using System.Collections.Generic;

namespace Application.Chatting.Core.Repsonses;

public class ContentResultV2
{
    public ContentResultV2()
    {
    }
    public PhotoResult Photo { get; set; }
    public string Text { get; set; }
    public bool Edited { get; set; }
    public Menu Menu { get; set; }
    public List<ContentResultV2> MultiMessages { get; set; }
    public bool IsEmpty { get; set; } = false;
}
public class Button
{
    public enum ButtonContentType
    {
        Text = 1,
        Url,
        CallbackData
    }
    public ButtonContentType Type { get; init; }
    public Button(string text, string url)
    {
        Type = ButtonContentType.Url;
        Text = text;
        Url = url;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="callbackData"></param>
    /// <param name="_">Sorry for this param,the c# compiler said that there is an existing constructor with the same signature</param>
    public Button(string text, string callbackData, bool _ = true)
    {
        Type = ButtonContentType.CallbackData;
        Text = text;
        CallbackData = callbackData;
    }
    public Button(string text)
    {
        Type = ButtonContentType.Text;
        Text = text;
    }
    public string Text { get; init; }
    public string Url { get; init; }
    public string CallbackData { get; init; }
}

public class Menu
{
    public enum MenuType
    {
        MessageMenu = 1,
        MenuKeyboard = 2,
    }
    public MenuType Type { get; init; }
    public Menu(MenuType menuType, IEnumerable<IEnumerable<Button>> menu)
    {
        Type = menuType;
        MenuScheme = menu;
    }
    public IEnumerable<IEnumerable<Button>> MenuScheme { get; init; }
}