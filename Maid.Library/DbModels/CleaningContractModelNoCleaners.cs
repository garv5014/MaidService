﻿using Postgrest.Attributes;
using Postgrest.Models;

namespace MaidService.Library.DbModels;
[Table("cleaning_contract")]
public class CleaningContractModelNoCleaners : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Column("cust_id")]
    public int Customer_Id { get; set; }

    [Column("date_completed")]
    public DateTime DateCompleted { get; set; }

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

    [Reference(typeof(CustomerModel))]
    public CustomerModel Customer { get; set; }

    [Reference(typeof(LocationModel))]
    public LocationModel Location { get; set; }

    [Reference(typeof(CleaningTypeModel))]
    public CleaningTypeModel CleaningType { get; set; }

}