using System.Diagnostics;
using System.Reflection;
using System.Text;
using ErpSystemOpgave.Data;

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

    // !! this is such an ugly hack. Simon says (heh) that I'm allowed to write shitty code.
    private bool Done;

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
    ///     var updatedCustomer = new EditScreen<Customer>("Edit Customer", db.GetCustomerFromId(1)!,
    ///         ("first name", "FirstName"),
    ///         ("last name", "LastName"),
    ///         ("Vej", "Address.Street"),
    ///         ("Nr.", "Address.HouseNumber"),
    ///         ("By", "Address.City"),
    ///         ("Phone", "ContactInfo.PhoneNumber"),
    ///         ("Mail", "ContactInfo.Email")).Show();
    ///     if(updatedCustomer is not null)
    ///         db.UpdateCustomer(updatedCustomer);
    ///     </code>
    /// </example>
    public EditScreen(string title, T record, params (string title, string property)[] props)
    {
        Title = title;
        InputFields = props.Select(p =>
        {
            var (target, prop) = Utility.GetProp(record!, p.property);
            PropertyInfo property = target.GetType().GetProperty(prop)!;
            var val = property.GetValue(target)!;
            if (val is Enum u)
                return CarouselWrapper.Create(p.title, p.property, u);
            return new InputField(
               p.title,
               p.property,
               val.ToString()!) as IField;
        }).ToList();
        InputFields.Add(new ButtonField("Okay", () =>
        {
            foreach (var item in InputFields)
                if (item is IInput e)
                    e.UpdateProperty(Record!);
            ReturnValue = Record;
            Done = true;
        }));
        InputFields.Add(new ButtonField("Tilbage", () => { Done = true; }));
        Record = record;
    }

    /// <summary>
    /// Start the "main loop" of the EditScreen.
    /// Draw the screen and await user input, and loop until the user leaves the screen.
    /// </summary>
    /// <returns>
    /// Either the modified `T` if terminated with "Ok". 
    /// otherwise return the original record unchanged.
    /// </returns>
    public T? Show()
    {
        ConsoleKeyInfo input = new();
        while (true)
        {
            var field = InputFields[SelectionIndex];
            switch (input.Key)
            {
                case ConsoleKey.DownArrow:
                    SelectionIndex.Rotate(1, InputFields.Count);
                    break;
                case ConsoleKey.UpArrow:
                    SelectionIndex.Rotate(-1, InputFields.Count);
                    break;
                default:
                    field.HandleInput(input);
                    break;
            }
            if (Done)
                break;
            Draw();
            input = Console.ReadKey();
        }
        Console.Clear();
        return ReturnValue;
    }

    private void Draw()
    {
        Console.Clear();
        System.Console.WriteLine(Title);
        System.Console.WriteLine("");
        foreach (var (item, i) in InputFields.Select((p, i) => (p, i)))
            item.Draw(i == SelectionIndex);
        if (InputFields[SelectionIndex] is InputField field)
            Console.SetCursorPosition(field.Value.Length + 2, SelectionIndex * 3 + 3);
        Console.CursorVisible = InputFields[SelectionIndex] is InputField;
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

/// <summary>
/// Little hack here. Since you don't get generic type inference on object instantiation, 
/// a Carousel<T> cannot be directly constructed from a reflected type.
/// As a workaround we use a "factory" with a static method which *is* subject to type inference.
/// 
/// Btw. did I ever mention how much I hate reflection?
/// </summary>
class CarouselWrapper
{
    public static Carousel<TWrap> Create<TWrap>(string title, string property, TWrap value)
    where TWrap : notnull, Enum
        => new(title, property, value);
}

class Carousel<T> : IField, IInput
where T : notnull, Enum
{
    public string Title { get; }
    public string Property { get; set; }
    public T Value { get; set; }
    object IInput.Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    private string[] variants;
    private int index;
    private int offset;
    private int cLeft, cTop;


    public Carousel(string title, string property, T value)
    {
        Title = title;
        Property = property;
        Value = value;
        variants = Enum.GetNames(value.GetType());
        index = Convert.ToInt32(Value);
    }

    public void Draw(bool focused)
    {
        (cLeft, cTop) = Console.GetCursorPosition();

        var (top, mid, bot, fill) = focused
        ? ("╔{0}╗", "║{0}║", "╚{0}╝", '═')
        : ("┌{0}┐", "│{0}│", "└{0}┘", '─');

        var t = string.Format("\x1b[4;34m{0}\x1b[0m", Title);
        var v = string.Join("", Enumerable
            .Range(0, 9)
            .Select(i => variants[(i + index).Mod(variants.Length)].PadCenter(14)))
            .Substring(14 + offset, 14 * 7)
            .PadCenter(98)
            .Insert(0, "\x1b[0m\x1b[2;37m")
            .Insert(25, "\x1b[0m\x1b[2;34m")
            .Insert(25 * 2, "\x1b[0m\x1b[0;34m")
            .Insert(25 * 3, "\x1b[0m \x1b[4;34m")
            .Insert(25 * 4 + 1, "\x1b[0m \x1b[0;34m")
            .Insert(25 * 5 + 2, "\x1b[0m\x1b[2;34m")
            .Insert(25 * 6 + 2, "\x1b[0m\x1b[2;37m")
            .Insert(25 * 7 + 2, "\x1b[0m");
        if (focused)
        {
            var sb = new StringBuilder(v);
            sb[25 * 3 + 4] = '«';
            sb[25 * 4 + 5] = '»';
            v = sb.ToString();
        }
        Console.WriteLine(top, t.PadRight(111, fill));
        Console.WriteLine(mid, v);
        Console.WriteLine(bot, new string(fill, 100));
    }

    public void HandleInput(ConsoleKeyInfo input)
    {
        switch (input.Key)
        {
            case ConsoleKey.LeftArrow:
                AnimateOffset(-14, 300.0f, Utility.EaseOutCube);
                index.Rotate(-1, variants.Length);
                break;
            case ConsoleKey.RightArrow:
                AnimateOffset(14, 300f, Utility.EaseOutCube);
                index.Rotate(1, variants.Length);
                break;
        }
    }
    private void AnimateOffset(int distance, float duration, Func<float, float> easingFunction)
    {
        double anim;
        var timer = new Stopwatch();
        timer.Start();
        while (offset != distance)
        {
            anim = easingFunction(timer.ElapsedMilliseconds / duration) * distance;
            if (MathF.Abs((float)anim) > MathF.Abs(offset))
            {
                Console.SetCursorPosition(cLeft, cTop);
                offset = (int)anim;
                Draw(true);
            }
        }
        offset = 0;

    }

    public void UpdateProperty(object obj)
    {
        var (target, prop) = Utility.GetProp(obj, Property);
        var property = target.GetType().GetProperty(prop)!;
        Value = (T)Enum.Parse(Value.GetType(), variants[index]);
        property.SetValue(target, Value);
    }

}


public interface IField
{
    public void Draw(bool focused);
    public void HandleInput(ConsoleKeyInfo input);
}

public interface IInput
{
    public object Value { get; set; }
    public string Property { get; set; }
    public void UpdateProperty(object target);
}

public static class Utility
{
    public static float EaseOutCube(float x) => 1 - MathF.Pow(1 - x, 3.0f);
    public static float EaseOutQuad(float x) => 1 - MathF.Pow(1 - x, 2.0f);

    public static (object, string) GetProp(object target, string property)
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

    public static int Mod(this int self, int mod)
    {
        return (self % mod + mod) % mod;
    }
    public static void Rotate(this ref int self, int increase, int max)
    {
        self += increase;
        self = (self % max + max) % max;
    }
    public static string PadCenter(this string self, int width)
    {
        return self.PadLeft(width - ((width - self.Length) / 2)).PadRight(width);
    }
    public static string PadCenter(this string self, int width, char fill)
    {
        return self.PadLeft(width - ((width - self.Length) / 2), fill).PadRight(width, fill);
    }
}