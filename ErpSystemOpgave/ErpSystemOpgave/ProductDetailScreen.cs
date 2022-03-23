using TECHCOOL.UI;

namespace ErpSystemOpgave;

public class ProductDetailScreen : Screen
{
    public override string Title { get; set; } = "Produkt detaljer";
    //Screen for P3

    protected override void Draw()
    {
        ListPage<ProductDetails> listDetails = new ListPage<ProductDetails>();
        
        listDetails.AddColumn("Varenr.", "ProductNumber");
    }
}