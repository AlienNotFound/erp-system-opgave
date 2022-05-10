using ErpSystemOpgave;
using TECHCOOL.UI;

namespace ErpSystemOpgave.Ui;

public class LandingPage
{
    private int SelectionIndex;
    private List<IField> InputFields = new();
    private List<String> Tooltips = new();

    public LandingPage()
    {
        InputFields.Add(new Button(
            "Produkter/Lager".PadCenter(30),
            () => Screen.Display(new ProductListScreen()),
            "\t\t\t"));
        InputFields.Add(new Button(
            "Kunder".PadCenter(30),
            () => Screen.Display(new CustomerListScreen()),
            "\t\t\t"));
        InputFields.Add(new Button(
            "Salg".PadCenter(30),
            () => Screen.Display(new OrderListScreen()),
            "\t\t\t"));

        Tooltips.Add("Inspicer lager.\nSe produktdetaljer\ntilfoj, opdater eller fjern produkter");
        Tooltips.Add("Se liste over kunder.\nSe kundedetaljer\ntilfoj, opdater eller fjern kunder");
        Tooltips.Add("Se ordrehistorik.\nInspicer ordrelinjer\ntilfoj, eller fjern ordre.");
    }

    private int Mod(int a, int b) => (a % b + b) % b;

    public void Show()
    {
        Console.CursorVisible = false;
        // init the `input` variable and read user input at the end of the loop
        // This is to make sure that the screen shows immediately rather than
        // waiting for the first input.
        var input = new ConsoleKeyInfo();
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
            Draw();
            input = Console.ReadKey();
        }
    }

    private void Draw()
    {
        Console.Clear();
        System.Console.Write("\x1b[1;34m");
        System.Console.WriteLine(@"
                   ____           _____       ______ _____  _____  
                  |  _ \   /\    / ____|     |  ____|  __ \|  __ \ 
                  | |_) | /  \  | (___ ______| |__  | |__) | |__) |
                  |  _ < / /\ \  \___ \______|  __| |  _  /|  ___/ 
                  | |_) / ____ \ ____) |     | |____| | \ \| |     
                  |____/_/    \_\_____/      |______|_|  \_\_|     
                  
                  ");
        System.Console.Write("\x1b[0m");

        foreach (var (item, i) in InputFields.Select((p, i) => (p, i)))
        {
            item.Draw(i == SelectionIndex);
        }

        var tooltip = Tooltips[SelectionIndex];
        var width = 50;

        var (top, mid, bot, fill) = ("\t\t╔{0}╗", "\t\t║ {0} ║", "\t\t╚{0}╝", '═');

        Console.WriteLine(top, new string(fill, width));
        foreach (var line in tooltip.Split('\n'))
        {
            Console.WriteLine(mid, line.PadCenter(width - 2));
        }
        Console.WriteLine(bot, new string(fill, width));
        Console.ResetColor();
    }


}