using ErpSystemOpgave.Data;
using TECHCOOL.UI;

namespace ErpSystemOpgave.Ui;

public class OrderEditScreen : EditScreen<SalesOrderHeader>
{
    public OrderEditScreen(string title, SalesOrderHeader record, params (string title, string property)[] props)
    : base(title, record, props)
    {
        InputFields.Insert(InputFields.Count - 2, new Button("Ordrelinier", () =>
        {
            Screen.Display(new OrderLineListScreen(record.OrderNumber));
        }));

    }
}