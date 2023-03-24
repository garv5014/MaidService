using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaidService.DbModels;
[Table("cleaning_contract")]
public class CleaningContract : BaseModel
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

    [Reference(typeof(Location))]
    public Location Location{ get; set; }

    [Reference(typeof(CleaningType))]
    public CleaningType CleaningType { get; set;}

}