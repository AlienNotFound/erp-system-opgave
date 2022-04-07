using System.Data.SqlClient;
using System;
using ErpSystemOpgave.Data;
using TECHCOOL.UI;

namespace ErpSystemOpgave;

public class ProductListScreen : Screen
{
    public override string Title { get; set; } = "Produkt liste";

    protected override void Draw()
    {
        //Guide: https://github.com/sinb-dev/TECHCOOL/tree/master/UI
        Clear(this);
        ListPage<Product> listPage = new ListPage<Product>();
        DataBase db = new DataBase();

        db.GetAllProducts();
    }
}