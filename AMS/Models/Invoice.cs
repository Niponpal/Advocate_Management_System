using AMS.BaseEntities;

public class Invoice:BaseEntity
{
  
    public string InvoiceNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime InvoiceDate { get; set; }

    public long ClientId { get; set; }
    public Client Client { get; set; }

   
}