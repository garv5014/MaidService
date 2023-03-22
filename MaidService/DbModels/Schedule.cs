using Postgrest.Attributes;

namespace MaidService.DbModels;
[Table("schedule")]
public class Schedule
{
    [PrimaryKey("id")]
    public int Id { get; set; }
    [Column("date")]
    public DateTime Date { get; set; }
    [Column("start_time")]
    public TimeSpan StartTime { get; set; }

    [Column("duration")]
    public TimeSpan Duration { get; set; }

}

//CREATE TABLE schedule(
//    id serial4 Not Null,
//    date DATE Not Null,
//    start_time TIME Not Null,
//    duration INTERVAL Not NULL,
//    CONSTRAINT schedule_pk PRIMARY KEY (id)
//);
