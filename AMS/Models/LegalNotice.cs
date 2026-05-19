public class LegalNotice
{
    public int Id { get; set; }

    public string NoticeTitle { get; set; }
    public string Description { get; set; }
    public DateTime NoticeDate { get; set; }

    public int ClientId { get; set; }
    public Client Client { get; set; }
}