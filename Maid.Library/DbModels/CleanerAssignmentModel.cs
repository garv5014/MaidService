using Postgrest.Attributes;
using Postgrest.Models;

namespace MaidService.Library.DbModels;
[Table("cleaner_assignments")]
public class CleanerAssignmentModel : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Reference(typeof(CleanerAvailabilityModel), shouldFilterTopLevel: false)]
    public CleanerAvailabilityModel Cleaner_Availability { get; set; }

}

public class CleanerAssignments
{
    public int Id { get; set; }
    public CleanerAvailability Cleaner_Availability { get; set; }
    public CleaningContract CleaningContract { get; set; }
}
