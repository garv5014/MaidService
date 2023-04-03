using Postgrest.Attributes;
using Postgrest.Models;

namespace MaidService.Library.DbModels;
[Table("contact_schedule")]
public class ContractScheduleModel : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }
    [Reference(typeof(ScheduleModel))]
    public ScheduleModel Schedule { get; set; }

    [Reference(typeof(CleaningContractModel))]
    public CleaningContractModel CleaningContract { get;set; }

}

public class ContractSchedule
{
    public int Id { get; set; }
    public Schedule Schedule { get; set; }
    public CleaningContract CleaningContract { get; set; }
}