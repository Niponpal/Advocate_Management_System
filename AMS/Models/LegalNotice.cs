using AMS.BaseEntities;
using AMS.Models;

public class LegalNotice:BaseEntity
{


    public string NoticeTitle { get; set; }
    public string Description { get; set; }
    public DateTime NoticeDate { get; set; }

    public long ClientId { get; set; }
    public Client Client { get; set; }
}