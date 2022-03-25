using System.Linq;
using ErpSystemOpgave.Data;
using TECHCOOL.UI;

namespace ErpSystemOpgave;

public class ProductUpdateScreen : Screen
{
    public override string Title { get => $"Update product: {_product.Name}"; set { } }

    private readonly Product _product;

    public ProductUpdateScreen(Product product)
    {
        _product = product;
    }

    protected override void Draw()
    {
        throw new NotImplementedException();
    }
}