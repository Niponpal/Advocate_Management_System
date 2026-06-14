using AMS.BaseEntities;

namespace AMS.Models;

public class Hearing: BaseEntity
{
    public DateTime HearingDate { get; set; }
    public string Remarks { get; set; }
    public string HearingStatus { get; set; }

    public long CaseId { get; set; }
    public Case Case { get; set; }

    public long CourtId { get; set; }
    public Court Court { get; set; }
}
