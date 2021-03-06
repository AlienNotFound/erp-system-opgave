using TECHCOOL.UI;

namespace ErpSystemOpgave;

using System.Reflection;
using ErpSystemOpgave.Ui;
using static System.Console;


class Program
{
    public static void Main(string[] args)
    {
        new LandingPage().Show();
    }

    public static void ShowMenu(params (String Description, Action Action)[] items)
    {
        ListPage<MenuItem> menu = new();
        menu.AddColumn("Action:", "Description");
        menu.Add(items.Select(i => new MenuItem(i.Description, i.Action)));
        menu.Select().Action.Invoke();
    }
    public static ListPage<T> CreateListPageWith<T>(IEnumerable<T> collection, params (string title, string property)[] items)
    {
        ListPage<T> listPage = new();
        listPage.Add(collection);
        foreach (var (title, property) in items)
        {
            listPage.AddColumnAligned(title, property, collection);
        }
        return listPage;
    }

    public static ListPage<DetailViewItem> CreateDetailsView<T>
    (T record, params (string title, string property)[] items)
    {
        var rows = items.Select(i => new DetailViewItem(
            i.title, typeof(T).GetProperty(i.property, System.Reflection.BindingFlags.FlattenHierarchy)?.GetValue(record)?.ToString() ?? ""));
        return CreateListPageWith(rows, ("Egenskab", "Title"), ("Værdi", "Value"));
    }
}

static class ListPageExtensions
{
    public static void AddColumnAligned<T>(this ListPage<T> listPage, string title, string property, IEnumerable<T> collection)
    {
        try
        {
            listPage.AddColumn(title, property, Math.Max(collection.Max(item =>
            String.Format("{0}", typeof(T).GetProperty(property, BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance)!.GetValue(item)).Length), title.Length) + 4);
        }
        catch (Exception e)
        {
            // throw new Exception(String.Format("Could not add column {0} to listpage of {1}: {2}", property, typeof(T), e));
            WriteLine("Could not add column {0} to listpage of {1}: {2}", property, typeof(T), e);
        }
    }
};

record MenuItem(string Description, Action Action);
record DetailViewItem(string Title, string Value);