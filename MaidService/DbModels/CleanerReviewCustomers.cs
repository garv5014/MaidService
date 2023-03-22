using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaidService.DbModels;
[Table("cleaner_review_cust")]
public class CleanerReviewCustomers : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }
    [Reference(typeof(Customer))]
    public Customer Customer { get; set; }
    [Reference(typeof(Cleaner))]
    public Cleaner Cleaner { get; set; }
    [Column("rating")]
    public int Rating { get; set; }
    [Column("review")]
    public string Review { get; set; }
}
