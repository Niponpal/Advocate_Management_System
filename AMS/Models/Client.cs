using AMS.BaseEntities;

namespace AMS.Models;

public class Client : BaseEntity
{
    public string ClientName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string NIDNumber { get; set; }

    public ICollection<Case> Cases { get; set; }

    public ICollection<Payment> Payments { get; set; }
    public ICollection<Appointment> Appointments { get; set; }
    public ICollection<Invoice> Invoices { get; set; }
    public ICollection<LegalNotice> LegalNotices { get; set; }
}
