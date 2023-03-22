using Postgrest.Attributes;
using Postgrest.Models;

namespace MaidService.DbModels;
[Table("contact_schedule")]
public class ContractSchedule : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }
    [Reference(typeof(Schedule))]
    public Schedule Schedule { get; set; }

    [Reference(typeof(CleaningContract))]
    public CleaningContract CleaningContract { get;set; }

}
