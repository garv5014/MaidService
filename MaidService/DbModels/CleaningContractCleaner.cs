using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MaidService.DbModels;
[Table("cc_cleaner")]
public class CleaningContractCleaner : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Reference(typeof(CleaningContract))]
    public CleaningContract Contract{ get; set; }

    [Reference(typeof(Cleaner))]
    public Cleaner Cleaner { get; set; }


}

