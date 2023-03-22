using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaidService.DbModels;

[Table("customer")]
public class Customer : BaseModel
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
