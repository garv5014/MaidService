using Postgrest.Attributes;
using Postgrest.Models;

namespace MaidService.Library.DbModels;
[Table("customer_payment")]
public class CustomerPaymentModel : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Reference(typeof(CustomerModel))]
    public CustomerModel Customer { get; set; }

    [Reference(typeof(CleaningContractModel))]
    public CleaningContractModel CleaningContract { get;set; }

    [Column("amount_paid")]
    public string AmountPaid { get; set; }

    [Column("received_date")]
    public DateTime ReceivedDate { get; set; }
}

public class CustomerPayment
{
    public int Id { get; set; }

    public Customer Customer { get; set; }

    public CleaningContract CleaningContract { get; set; }

    public string AmountPaid { get; set; }

    public DateTime ReceivedDate { get; set; }
}