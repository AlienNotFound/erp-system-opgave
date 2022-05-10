using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace ErpSystemOpgave.Ui;

public class Menu<T>
{

    protected int SelectionIndex;
    public List<IField> InputFields = new();
    protected readonly string Title;
    public bool Done;
    public T? ReturnValue;

    public Menu(string title)
    {
        Title = title;
    }

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

    protected void Draw()
    {
        Console.Clear();
        Console.WriteLine(Title);
        Console.WriteLine("");
        foreach (var (item, i) in InputFields.Select((p, i) => (p, i)))
            item.Draw(i == SelectionIndex);
        if (InputFields[SelectionIndex] is InputField field)
            Console.SetCursorPosition(field.Value.Length + 2, SelectionIndex * 3 + 3);
        Console.CursorVisible = InputFields[SelectionIndex] is InputField;
    }

}