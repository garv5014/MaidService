using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaidService.DbModels;
[Table("customer_payment")]
public class CustomerPayment : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Reference(typeof(Customer))]
    public Customer Customer { get; set; }

    [Reference(typeof(CleaningContract))]
    public CleaningContract CleaningContract { get;set; }

    [Column("amount_paid")]
    public int AmountPaid { get; set; }

    [Column("received_date")]
    public DateTime ReceivedDate { get; set; }
}

