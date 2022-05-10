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
        var Id = reader.GetInt32(offset++);
        var Name = reader.GetString(offset++);
        var AddressId = reader.GetInt32(offset++);
        var Currency = reader.GetString(offset++);
        return new()
        {
            Id = reader.GetInt32(offset++),
            Name = reader.GetString(offset++),
            Address = DataBase.Instance.GetAddressById(AddressId)!,
            AddressId = AddressId,
            Currency = reader.GetString(offset++)
        };
    }


    public int Id { get; set; } = default;
    public int AddressId { get; set; } = default;

    public Address Address { get; set; } = new();
    public string Name { get; set; } = "";
    public string Currency { get; set; } = "";

    public SqlParameter[] SqlParameters => new SqlParameter[]{
        new("@id", Id),
        new("@name", Name),
        new("@addressId", Address.Id),
        new("@currency", Currency)
    };

    public Company() { }

}