using Postgrest.Attributes;
using Postgrest.Models;

namespace MaidService.Library.DbModels;
[Table("cleaner")]
public class CleanerModel : BaseModel
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

    [Column("bio")]
    public string Bio { get; set; }

    [Column("verified")]
    public bool Verified { get; set; }

    [Column("hire_date")]
    public DateTime HireDate { get; set; }

    [Column("auth_id")]
    public string AuthId { get; set; }

    //[Reference(typeof(CleaningContractModel), includeInQuery: false)]    
    //public List<CleaningContractModel> CleaningContract { get; set; }

}

public class Cleaner
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string PhoneNumber { get; set; }
    public string Bio { get; set; }
    public bool Verified { get; set; }
    public DateTime HireDate { get; set; }
    public string AuthId { get; set; }

    public string FullName => $"{FirstName} {Surname}";
}