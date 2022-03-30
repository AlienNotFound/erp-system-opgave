using ErpSystemOpgave;
using ErpSystemOpgave.Data;
using TECHCOOL.UI;

public class Program
{
    public static void Main(string[] args)
    {
        SalesOrderHearderScreen salesOrderHearderScreen = new SalesOrderHearderScreen();
        Screen.Display(salesOrderHearderScreen);
    }
}