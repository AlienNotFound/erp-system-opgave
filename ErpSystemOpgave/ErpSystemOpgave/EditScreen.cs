namespace ErpSystemOpgave;


/// <summary>
/// A screen that allows editing an entry of `T`
/// </summary>
/// <typeparam name="T">foo</typeparam>
public class EditScreen<T>
{
    private int SelectionIndex;
    private readonly List<IField> InputFields;
    private readonly T Record;
    private readonly string Title;
    private T? ReturnValue;

    /// <summary>
    /// Create a new EditScreen.
    /// </summary>
    /// <param name="title">The title shown at the top of the menu</param>
    /// <param name="record">An instance of type `T`</param>
    /// <param name="props">
    /// A series of (title, propertyName)-tuples. Each item will become an input field
    /// with the label `title` and modify the property `propertyName`
    /// </param>
    /// <example>
    ///     <code>
    ///     var editScreen = new EditScreen<Customer>("Edit Customer", db.GetCustomerFromId(1)!,
    ///     ("first name", "FirstName"),
    ///     ("last name", "LastName"),
    ///     ("Vej", "Address.Street"),
    ///     ("Nr.", "Address.HouseNumber"),
    ///     ("By", "Address.City"),
    ///     ("Phone", "ContactInfo.PhoneNumber"),
    ///     ("Mail", "ContactInfo.Email"));
    ///     editScreen.Show();
    ///     </code>
    /// </example>
    public EditScreen(string title, T record, params (string title, string property)[] props)
    {

        Title = title;
        InputFields = props.Select(p => new InputField(
            p.title,
            p.property,
            ExpandProp(record!, p.property)) as IField).ToList();
        InputFields.Add(new ButtonField("Okay", () => ReturnValue = BuildReturn()));
        InputFields.Add(new ButtonField("Tilbage", () => ReturnValue = record));
        Record = record;
        System.Diagnostics.Debug.WriteLine($"Created edit screen with title: \"{title}\" for {record} with params: {props}");
    }

    /// <summary>
    /// Recursively "Expand" a property from name so it can be assigned to.
    /// </summary>
    /// <param name="target">T</param>
    /// <param name="property"></param>
    /// <returns></returns>
    private string ExpandProp(object target, string property)
    {
        // TODO: `ExpandProp` and `GetProp` largely do the same. just merge them.
        string[] props = property.Split('.');
        if (target.GetType().GetProperty(props[0])?.GetValue(target) is object newTarget)
        {
            return props.Length > 1
            ? ExpandProp(newTarget, string.Join('.', props[1..]))
            : newTarget.ToString()!;
        }
        throw new Exception("foo");
    }

    private (object, string) GetProp(object target, string property)
    {
        string[] props = property.Split('.');
        if (props.Length == 1)
            return (target, property);

        if (target.GetType().GetProperty(props[0])?.GetValue(target) is object newTarget)
        {
            return GetProp(newTarget, string.Join('.', props[1..]));
        }
        throw new Exception("Looks like you fucked up");
    }

    private T BuildReturn()
    {
        foreach (var item in InputFields)
        {
            if (item is InputField field)
            {
                System.Console.WriteLine("build prop: {0}", field.Property);
                var (target, prop) = GetProp(Record!, field.Property);
                target.GetType().GetProperty(prop)?.SetValue(target, field.Value);
            }
        }
        return Record;
    }

    /// <summary>
    /// Start the "main loop" of the EditScreen.
    /// Draw the screen and await user input, and loop until the user leaves the screen.
    /// </summary>
    /// <returns>
    /// Either the modified `T` if terminated with "Ok". 
    /// otherwise return the original record unchanged.
    /// </returns>
    public T Show()
    {
        ConsoleKeyInfo input = new();
        while (true)
        {
            var field = InputFields[SelectionIndex];
            switch (input.Key)
            {
                case ConsoleKey.DownArrow:
                    SelectionIndex = Mod(SelectionIndex + 1, InputFields.Count);
                    break;
                case ConsoleKey.UpArrow:
                    SelectionIndex = Mod(SelectionIndex - 1, InputFields.Count);
                    break;
                default:
                    field.HandleInput(input);
                    break;
            }
            if (ReturnValue is not null)
                return ReturnValue;
            Draw();
            input = Console.ReadKey();
        }
    }

    private int Mod(int a, int b) => (a % b + b) % b;


    private void Draw()
    {
        Console.Clear();
        System.Console.WriteLine(Title);
        System.Console.WriteLine("");
        foreach (var (item, i) in InputFields.Select((p, i) => (p, i)))
        {
            item.Draw(i == SelectionIndex);
        }
        if (InputFields[SelectionIndex] is InputField field)
        {
            Console.CursorVisible = true;
            Console.SetCursorPosition(field.Value.Length + 2, SelectionIndex * 3 + 3);
        }
        else
        {
            Console.CursorVisible = false;
        }
    }
}

public class ButtonField : IField
{
    public ButtonField(string title, Action action)
    {
        Title = title;
        Action = action;
        Indent = "";
    }
    public ButtonField(string title, Action action, string indent)
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
        var (top, mid, bot, fill) = (Indent + "╔{0}╗", Indent + "║ {0} ║", Indent + "╚{0}╝", '═');
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

class InputField : IField
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

    public string Title { get; }
    public string Property { get; }
    public string Value { get; set; }
}

public interface IField
{
    public void Draw(bool focused);
    public void HandleInput(ConsoleKeyInfo input);
}