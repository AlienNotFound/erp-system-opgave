using System.Diagnostics;
using System.Text;

namespace ErpSystemOpgave.Ui;

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