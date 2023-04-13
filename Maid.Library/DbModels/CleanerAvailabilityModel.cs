using Postgrest.Attributes;
using Postgrest.Models;

namespace MaidService.Library.DbModels;
[Table("cleaner_availability")]
public class CleanerAvailabilityModel : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Column("schedule_id")]
    public ScheduleModel Schedule { get; set; }

    [Column("cleaner_id")]
    public CleanerModel Cleaner { get; set; }

    [Reference(typeof(CleanerAssignmentModel))]
    public List<CleanerAssignmentModel> CleanerAssignments { get; set; }
}

public class CleanerAvailability
{
    public int Id { get; set; }

    public Schedule Schedule { get; set; }

    public Cleaner Cleaner { get; set; }
}

