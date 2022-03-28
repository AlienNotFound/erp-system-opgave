using System.Data.SqlClient;
using TECHCOOL.UI;

namespace ErpSystemOpgave;

public class ProductListScreen : Screen
{
    public override string Title { get; set; } = "Produkt liste";

    protected override void Draw()
    {
        //Guide: https://github.com/sinb-dev/TECHCOOL/tree/master/UI
        Clear(this);
        ListPage<ProductDetails> listPage = new ListPage<ProductDetails>();
        DataBase db = new DataBase();

        db.GetAllProducts();
        
        /*Menu menu = new Menu();

        menu.Add(new ProductDetailScreen());
        menu.Start(this);*/
    }
}