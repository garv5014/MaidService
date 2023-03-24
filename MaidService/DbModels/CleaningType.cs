using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaidService.DbModels;
[Table("cleaning_type")]
public class CleaningType : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Column("type")]
    public string Type { get; set; }
    [Column("description")] 
    public string Description { get; set; }
}
