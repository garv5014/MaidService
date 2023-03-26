using Postgrest.Attributes;
using Postgrest.Models;

namespace MaidService.Library.DbModels;

[Table("customer")]
public class CustomerModel : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Column("email")]
    public string Email { get; set; }

    [Column("firstname")]
    public string FirstName { get; set; }

    [Column("surname")]
    public string Surname { get; set; }

    [Column("phone_number")]
    public string PhoneNumber { get; set; }
}

public class Customer
{
    public int Id { get; set; }

    public string Email { get; set; }

    public string FirstName { get; set; }

    public string Surname { get; set; }

    public string PhoneNumber { get; set; }
}