using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaidService.DbModels;
[Table("location")]
public class Location : BaseModel
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
