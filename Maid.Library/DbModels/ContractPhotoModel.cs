using MaidService.Library.DbModels;
using Postgrest.Attributes;
using Postgrest.Models;

namespace MaidService.Library.DbModels;
[Table("contract_photos")]
public class ContractPhotoModel : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Column("photo_url")]
    public string PhotoUrl { get; set; }

    [Column("cleaning_contract_id")]
    public int ContractId { get; set; }
}

public class ContractPhoto
{
    public int Id { get; set; }
    public string Filename { get; set; }
    public int ContractId { get; set; }
}
