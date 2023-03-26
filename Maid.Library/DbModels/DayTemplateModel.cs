using Postgrest.Attributes;
using Postgrest.Models;

namespace MaidService.Library.DbModels;
[Table("day_template")]
public class DayTemplateModel : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set;}

    [Column("start_time")]
    public TimeSpan StartTime { get; set;}

    [Column("duration")]
    public TimeSpan Duration { get; set;}
}

public class DayTemplate
{
    public int Id { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan Duration { get; set; }

}