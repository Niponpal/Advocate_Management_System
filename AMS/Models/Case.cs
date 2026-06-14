using AMS.BaseEntities;
using AMS.Models.Auth;

namespace AMS.Models;

public class Case : BaseEntity
{
    public string CaseNumber { get; set; }
    public string CaseTitle { get; set; }
    public string CaseType { get; set; }
    public string Description { get; set; }

    public DateTime FilingDate { get; set; }
    public string Status { get; set; }

    public long AdvocateId { get; set; }
    public Advocate Advocate { get; set; }

    public long ClientId { get; set; }
    public Client Client { get; set; }

    public ICollection<Hearing> Hearings { get; set; }
    public ICollection<Document> Documents { get; set; }
}
