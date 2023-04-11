using Postgrest.Attributes;
using Postgrest.Models;

namespace MaidService.Library.DbModels;
[Table("cleaner_availability")]
public class CleanerAvailabilityModel : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Reference(typeof(ScheduleModel))]
    public CleaningContractModel Schedule{ get; set; }

    [Reference(typeof(CleanerModel))]
    public CleanerModel Cleaner { get; set; }
}

public class CleanerAvailability
{
    public int Id { get; set; }

    public Schedule Schedule { get; set; }

    public Cleaner Cleaner { get; set; }
}
