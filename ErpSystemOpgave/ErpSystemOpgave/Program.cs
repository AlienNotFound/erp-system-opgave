using ErpSystemOpgave;
using ErpSystemOpgave.Data;
using TECHCOOL.UI;
public class Program
{
    public static void Main(string[] args)
    {
        ProductListScreen productListScreen = new ProductListScreen();
        Screen.Display(productListScreen);
        // ProductDetailScreen detailScreen = new(
        //     new Product(
        //         42069,
        //         "Half a unicorn",
        //         "Unicorn is length 0.5???",
        //         (decimal)80085.0,
        //         (decimal)8008135.0,
        //         10.0,
        //         "n008",
        //         ProductUnit.Quantity));
        // Screen.Display(detailScreen);
    }
}