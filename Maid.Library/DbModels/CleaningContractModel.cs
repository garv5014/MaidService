using Postgrest.Attributes;
using Postgrest.Models;
using System;

namespace MaidService.Library.DbModels;
[Table("cleaning_contract")]
public class CleaningContractModel : BaseModel
{
    [PrimaryKey("id")] 
    public int Id { get; set; }

    [Column("cust_id")]
    public int Customer_Id { get; set; }

    [Column("date_completed")]
    public DateTime? DateCompleted { get; set; }

    [Column("schedule_date")]
    public DateTime ScheduleDate { get; set; }

    [Column("cost")]
    public string Cost { get; set; }

    [Column("requested_hours")]
    public TimeSpan RequestedHours { get; set; }

    [Column("est_sqft")]
    public int EstSqft { get; set; }

    [Column("num_of_cleaners")]
    public int NumOfCleaners { get; set; }

    [Column("notes")]
    public string Notes { get; set; }

    [Column("cleaning_type_id")]
    public int CleaningTypeId { get; set; }

    [Column("location_id")]
    public int LocationId { get; set; }

    [Reference(typeof(CleanerAvailabilityModel), shouldFilterTopLevel: false)]
     public List<CleanerAvailabilityModel> Cleaners { get; set; }

    [Reference(typeof(CustomerModel))]
    public CustomerModel Customer { get; set; }

    [Reference(typeof(LocationModel))]
    public LocationModel Location{ get; set; }

    [Reference(typeof(CleaningTypeModel))]
    public CleaningTypeModel CleaningType { get; set;}

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

    public CleaningLocation Location { get; set; } = new();

    public CleaningType CleaningType { get; set; }

    public List<CleanerAvailability> Cleaners { get; set; }
}
