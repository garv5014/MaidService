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
    public string SurName { get; set; }

    [Column("phone_number")]
    public string PhoneNumber { get; set; }
    [Column("auth_id")]
    public string AuthId { get; set; }

    [Column("pfp_url")]
    public string ProfilePicture { get; set; }
}

public class Customer : PublicUser
{
}