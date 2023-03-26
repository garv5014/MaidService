using Postgrest.Attributes;
using Postgrest.Models;

namespace MaidService.Library.DbModels;
[Table("cleaning_type")]
public class CleaningTypeModel : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Column("type")]
    public string Type { get; set; }
    [Column("description")]
    public string Description { get; set; }
}

public class CleaningType
{
    public int Id { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
}
