using Postgrest.Attributes;
using Postgrest.Models;

namespace MaidService.Library.DbModels;
[Table("cleaning_contract")]
public class CleaningContractModel : CleaningContractModelNoCleaners
{

    [Reference(typeof(CleanerModel))]
    public List<CleanerModel> Cleaners { get; set; }

}

public class CleaningContract 
{
    public int Id { get; set; }

    public int Customer_Id { get; set; }

    public DateTime DateCompleted { get; set; }

    public DateTime ScheduleDate { get; set; }

    public string Cost { get; set; }

    public TimeSpan RequestedHours { get; set; }

    public int EstSqft { get; set; }

    public int NumOfCleaners { get; set; }

    public string Notes { get; set; }

    public CleaningLocation Location { get; set; }

    public CleaningType CleaningType { get; set; }

    public List<Cleaner> Cleaners { get; set; }
}
