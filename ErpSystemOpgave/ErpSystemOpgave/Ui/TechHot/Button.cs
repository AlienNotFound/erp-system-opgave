namespace ErpSystemOpgave.Ui;

public class Button : IField
{
    public Button(string title, Action action)
    {
        Title = title;
        Action = action;
        Indent = "";
    }
    public Button(string title, Action action, string indent)
    {
        Title = title;
        Action = action;
        Indent = indent;
    }

    public string Title { get; }
    public Action Action { get; }
    public string Indent { get; set; }

    public void Draw(bool focused)
    {
        var (top, mid, bot, fill) = !focused ? (Indent + "╔{0}╗", Indent + "║ {0} ║", Indent + "╚{0}╝", '═') : (Indent + " {0} ", Indent + "  {0}  ", Indent + " {0} ", ' ');
        if (focused)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
        }

        var width = Title.Length + 2;

        Console.WriteLine(top, new String(fill, width));
        Console.WriteLine(mid, Title);
        Console.WriteLine(bot, new String(fill, width));
        Console.ResetColor();

    }

    public void HandleInput(ConsoleKeyInfo input)
    {
        if (input.Key == ConsoleKey.Enter)
        {
            Action.Invoke();
        }
    }
}