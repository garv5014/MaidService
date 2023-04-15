using Postgrest.Attributes;
using Postgrest.Models;

namespace MaidService.Library.DbModels;
[Table("schedule")]
public class ScheduleModel : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Column("date")]
    public DateTime Date { get; set; }

    [Column("start_time")]
    public DateTime StartTime { get; set; }

    [Column("duration")]
    public TimeSpan Duration { get; set; }
}


public class Schedule 
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public DateTime StartTime { get; set; }
    public TimeSpan Duration { get; set; }
}

public class ScheduleEqualityComparer : IEqualityComparer<Schedule>
{
    public bool Equals(Schedule x, Schedule y)
    {
        return x.Id == y.Id;
    }
    public int GetHashCode(Schedule obj)
    {
        return obj.Id.GetHashCode();
    }
}   

//CREATE TABLE schedule(
//    id serial4 Not Null,
//    date DATE Not Null,
//    start_time TIME Not Null,
//    duration INTERVAL Not NULL,
//    CONSTRAINT schedule_pk PRIMARY KEY (id)
//);
