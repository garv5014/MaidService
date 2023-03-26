using Postgrest.Attributes;
using Postgrest.Models;

namespace MaidService.Library.DbModels;
[Table("location")]
public class LocationModel : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Column("address")]
    public string Address { get; set; }
    [Column("state")]
    public string State { get; set; }

    [Column("zipcode")]
    public string Zipcode { get; set; }

    [Column("apartment_number")]
    public string Apartment_number { get; set; }
}

public class Location
{
    public int Id { get; set; }

    public string Address { get; set; }

    public string State { get; set; }

    public string Zipcode { get; set; }

    public string Apartment_number { get; set; }
}
