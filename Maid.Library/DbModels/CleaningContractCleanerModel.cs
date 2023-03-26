using Postgrest.Attributes;
using Postgrest.Models;

namespace MaidService.Library.DbModels;
[Table("cc_cleaner")]
public class CleaningContractCleanerModel : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Reference(typeof(CleaningContractModel))]
    public CleaningContractModel Contract{ get; set; }

    [Reference(typeof(CleanerModel))]
    public CleanerModel Cleaner { get; set; }


}

public class CleaningContractCleaner
{
    public int Id { get; set; }

    public CleaningContract Contract { get; set; }

    public Cleaner Cleaner { get; set; }
}
