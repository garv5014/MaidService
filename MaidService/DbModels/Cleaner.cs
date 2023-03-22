using Postgrest.Attributes;
using Postgrest.Models;

namespace MaidService.DbModels;
[Table("cleaner")]
public class Cleaner : BaseModel
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

    [Column("bio")]
    public string Bio { get; set; }

    [Column("verified")]
    public bool Verified { get; set; }

    [Column("hire_date")]
    public DateTime HireDate { get; set; }
}
