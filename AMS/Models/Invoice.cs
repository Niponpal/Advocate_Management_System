public class Invoice
{
    public int Id { get; set; }

    public string InvoiceNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime InvoiceDate { get; set; }

    public int ClientId { get; set; }
    public Client Client { get; set; }
}