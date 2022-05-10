namespace ErpSystemOpgave.Ui;

class InputField : IField, IInput
{
    public InputField(string Title, string Property, string Value)
    {
        this.Title = Title;
        this.Property = Property;
        this.Value = Value;
    }

    public void Draw(bool focused)
    {
        var (top, mid, bot, fill) = focused
        ? ("╔{0}╗", "║ {0}║", "╚{0}╝", '═')
        // : ("┌{0}┐", "│{0}│", "└{0}┘", '─');
        // : ("", " {0} ", " {0} ", '─');
        : ("╔{0}╗", "║ {0}║", "╚{0}╝", '═');
        var t = string.Format("\x1b[4;34m{0}\x1b[0m", Title);
        Console.WriteLine(top, t.PadRight(111, fill));
        Console.WriteLine(mid, Value.PadRight(99));
        Console.WriteLine(bot, new String(fill, 100));
    }

    public void HandleInput(ConsoleKeyInfo input)
    {
        switch (input.Key)
        {

            case ConsoleKey.Backspace:
                if (!string.IsNullOrEmpty(Value))
                    Value = Value[..^1];
                break;
            default:
                var ch = input.KeyChar;
                if (!Char.IsControl(ch) || ch == ' ')
                    Value += ch;
                break;
        }
    }

    public void UpdateProperty(object obj)
    {
        var (target, prop) = Utility.GetProp(obj, Property);
        var property = target.GetType().GetProperty(prop)!;
        object newValue = property.GetValue(target) switch
        {
            string => Value,
            int => int.Parse(Value),
            short => short.Parse(Value),
            double => double.Parse(Value),
            decimal => decimal.Parse(Value),
            _ => throw new Exception($"type not supported: {property.PropertyType}")
        };
        property.SetValue(target, newValue);
    }

    public string Title { get; }
    public string Property { get; }
    public string Value { get; set; }
    object IInput.Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    string IInput.Property { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}