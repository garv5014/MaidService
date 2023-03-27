using Postgrest.Attributes;
using Postgrest.Models;

namespace MaidService.Library.DbModels;
[Table("cust_review_cleaner")]
public class CustomerReviewCleanerModel : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Reference(typeof(CustomerModel))]
    public CustomerModel Customer { get; set; }
    [Reference(typeof(CleanerModel))]
    public CleanerModel Cleaner { get; set; }
    [Column("rating")]
    public int Rating { get; set; }
    [Column("review")]
    public string Review { get; set;}
}

public class CustomerReviewCleaners
{
    public int Id { get; set; }
    public Customer Customer { get; set; }
    public Cleaner Cleaner { get; set; }
    public int Rating { get; set; }
    public string Review { get; set; }
}