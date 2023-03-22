using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaidService.DbModels;
[Table("day_template")]
public class DayTemplate : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set;}

    [Column("start_time")]
    public TimeSpan StartTime { get; set;}

    [Column("duration")]
    public TimeSpan Duration { get; set;}
}
//CREATE TABLE day_template(
//    id serial4 Not Null,
//    start_time time Not Null,
//    duration Interval Not Null,
//    CONSTRAINT day_template_pk PRIMARY KEY (id)
//);