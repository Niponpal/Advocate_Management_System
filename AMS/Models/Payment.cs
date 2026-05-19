public class Payment
{
    public int Id { get; set; }

    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentStatus { get; set; }

    public int ClientId { get; set; }
    public Client Client { get; set; }

    public int CaseId { get; set; }
    public Case Case { get; set; }
}