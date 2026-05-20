using AMS.BaseEntities;

public class Payment :BaseEntity
{

    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentStatus { get; set; }

    public long ClientId { get; set; }
    public Client Client { get; set; }

    public long CaseId { get; set; }
    public Case Case { get; set; }
}