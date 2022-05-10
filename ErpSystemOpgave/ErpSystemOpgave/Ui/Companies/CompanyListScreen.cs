using ErpSystemOpgave.Data;
using TECHCOOL.UI;

namespace ErpSystemOpgave.Ui;

public class CompanyListScreen : Screen
{
    public static int SelectedId;
    public override string Title { get; set; } = "Virksomheder";

    private ListPage<Company> listPage;

    public CompanyListScreen()
    {
        Clear(this);
        listPage = Program.CreateListPageWith(
            DataBase.Instance.GetAllCompanies(),
            ("Navn", "Name"),
            ("Adresse", "Address"),
            ("Valuta", "Currency"));

        Console.WriteLine("\nTryk på ENTER på den valgte Virksomhed, for at se detaljer\nTryk F2 for at redigere Virksomhed");
        listPage.AddKey(ConsoleKey.F2, c =>
        {
            Clear();
            if (DataBase.Instance.GetCompanyById(c.Id) is Company p)
                if (new EditScreen<Company>("Rediger Virksomhed", p,
                    ("Navn", "Name"),
                    ("Valuta", "Currency"),
                    ("Adresse ID", "AddressId"))
                .Show() is Company updated)
                    DataBase.Instance.UpdateCompany(updated);
        });
        listPage.AddKey(ConsoleKey.F1, _ =>
        {
            Clear();
            if (new EditScreen<Company>("Rediger Virksomhed", new Company(),
                ("Navn", "Name"),
                ("Valuta", "Currency"),
                ("Adresse ID", "AddressId"))
            .Show() is Company updated)
                DataBase.Instance.UpdateCompany(updated);
        });
    }

    protected override void Draw()
    {

        if (listPage.Select() is Company selected)
        {
            Clear();
            Display(new CustomerDetailsScreen(selected.Id));
        }
    }
}