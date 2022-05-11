using System.Data;
using System.Data.SqlClient;

namespace ErpSystemOpgave.Data;

public class Company
{
    public static Company FromReader(IDataReader reader)
    {
        var offset = 0;
        return FromReader(reader, ref offset);
    }

    public static Company FromReader(IDataReader reader, ref int offset)
    {
        var id = reader.GetInt32(offset++);
        var name = reader.GetString(offset++);
        var addressId = reader.GetInt32(offset++);
        var currency = reader.GetString(offset++);
        return new Company {
            Id = id,
            Name = name,
            Address = DataBase.Instance.GetAddressById(addressId)!,
            AddressId = addressId,
            Currency = currency
        };
    }


    public int Id { get; set; }
    public int AddressId { get; set; }

    public Address Address { get; set; } = new();
    public string Name { get; set; } = "";
    public string Currency { get; set; } = "";

    public SqlParameter[] SqlParameters => new SqlParameter[]{
        new("@id", Id),
        new("@name", Name),
        new("@addressId", AddressId),
        new("@currency", Currency)
    };
}