using System.Linq;
using ErpSystemOpgave;
using System;
using ErpSystemOpgave.Data;
using TECHCOOL.UI;

namespace ErpSystemOpgave;

public class ProductDetailScreen : Screen
{
    public int ProductId = ProductListScreen.SelectedId;
    public override string Title { get; set; } = "Produkt detaljer";
    
    public readonly Product _product;
    public ProductDetailScreen(Product product)
    {
        _product = product;
    }
    protected override void Draw()
    {
        //? den underlige format string er blot for farvens skyld.
        //? det kan betragtes som at det er "{0,-30} {1}" hvilket betyder
        //? "felt 0 med 30 characters left padding efterfulgt af felt 1"
        Console.WriteLine("{0,-30} \x1b[32m{1}\x1b[0m", "varenr:", _product.ProductId);
        Console.WriteLine("{0,-30} \x1b[32m{1}\x1b[0m", "Navn:", _product.Name);
        Console.WriteLine("{0,-30} \x1b[32m{1}\x1b[0m", "Beskrivelse:", _product.Description);
        Console.WriteLine("{0,-30} \x1b[32m{1}\x1b[0m", "Salgspris:", _product.SalePrice);
        Console.WriteLine("{0,-30} \x1b[32m{1}\x1b[0m", "Indkoebspris:", _product.BuyPrice);
        Console.WriteLine("{0,-30} \x1b[32m{1}\x1b[0m", "Paa Lager:", _product.InStock);
        Console.WriteLine("{0,-30} \x1b[32m{1}\x1b[0m", "Lokation: ", _product.Location);
        Console.WriteLine("{0,-30} \x1b[32m{1}\x1b[0m", "Enhed:", _product.Unit);

        // !! Dette er lidt et hack. Her bruger vi en `ListPage` som menu,
        // !! da den dedikerede `Menu` rydder skaermen.
        Program.ShowMenu(
            ("Go back", () => this.Quit()),
            ("Update", () => Screen.Display(new ProductUpdateScreen(_product)))
        );
    }
}

