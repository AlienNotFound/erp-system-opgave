using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace ErpSystemOpgave.Ui;


/// <summary>
/// A screen that allows editing an entry of `T`
/// </summary>
/// <typeparam name="T">foo</typeparam>
public class EditScreen<T> : Menu<T>
{
    private readonly T Record;

    /// <summary>
    /// Create a new EditScreen.
    /// </summary>
    /// <param name="title">The title shown at the top of the menu</param>
    /// <param name="record">An instance of type `T`</param>
    /// <param name="props">
    /// A series of (title, propertyName)-tuples. Each item will become an input field
    /// with the label `title` and modify the property `propertyName`
    /// </param>
    /// <example>
    ///     <code>
    ///     var updatedCustomer = new EditScreen<Customer>("Edit Customer", db.GetCustomerFromId(1)!,
    ///         ("first name", "FirstName"),
    ///         ("last name", "LastName"),
    ///         ("Vej", "Address.Street"),
    ///         ("Nr.", "Address.HouseNumber"),
    ///         ("By", "Address.City"),
    ///         ("Phone", "ContactInfo.PhoneNumber"),
    ///         ("Mail", "ContactInfo.Email")).Show();
    ///     if(updatedCustomer is not null)
    ///         db.UpdateCustomer(updatedCustomer);
    ///     </code>
    /// </example>
    public EditScreen(string title, T record, params (string title, string property)[] props) : base(title)
    {
        InputFields = props.Select(p =>
        {
            var (target, prop) = Utility.GetProp(record!, p.property);
            PropertyInfo property = target.GetType().GetProperty(prop)!;
            try
            {
                var val = property.GetValue(target)!;
                if (val is Enum u)
                    return CarouselWrapper.Create(p.title, p.property, u);
                return new InputField(
                   p.title,
                   p.property,
                   val.ToString()!) as IField;
            }
            catch (Exception e)
            {
                throw new Exception($"No value for property {p}", e);
            }
        }).ToList();
        InputFields.Add(new Button("Okay", () =>
        {
            foreach (var item in InputFields)
                if (item is IInput e)
                    e.UpdateProperty(Record!);
            ReturnValue = Record;
            Done = true;
        }));
        InputFields.Add(new Button("Tilbage", () => { Done = true; }));
        Record = record;
    }
}


public interface IField
{
    public void Draw(bool focused);
    public void HandleInput(ConsoleKeyInfo input);
}

public interface IInput
{
    public object Value { get; set; }
    public string Property { get; set; }
    public void UpdateProperty(object target);
}
