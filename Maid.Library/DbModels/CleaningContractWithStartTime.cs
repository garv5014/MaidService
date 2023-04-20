using Newtonsoft.Json;

namespace MaidService.Library.DbModels;

public class CleaningContractWithStartTime
{
    public int? Id { get; set; }
    [JsonProperty("cust_id")]
    public int? CustomerId { get; set; }
    [JsonProperty("date_completed")]
    public DateTime? DateCompleted { get; set; }
    [JsonProperty("schedule_date")]
    public DateTime ScheduleDate { get; set; }
    public string? Cost { get; set; }
    [JsonProperty("requested_hours")]
    public TimeSpan RequestedHours { get; set; }
    [JsonProperty("est_sqft")]
    public int? EstSqft { get; set; }
    [JsonProperty("num_of_cleaners")]
    public int? NumOfCleaners { get; set; }
    public string? Notes { get; set; }
    [JsonProperty("location_id")]
    public int? LocationId { get; set; }
    [JsonProperty("cleaning_type_id")]
    public int? CleaningTypeId { get; set; }
    [JsonProperty("start_time")]
    public TimeSpan StartTime { get; set; }

    [JsonProperty("cleaning_type")]
    public string? CleaningType { get; set; }
}
