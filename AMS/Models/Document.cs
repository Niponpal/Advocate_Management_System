public class Document
{
    public int Id { get; set; }

    public string DocumentTitle { get; set; }
    public string FilePath { get; set; }
    public string DocumentType { get; set; }

    public DateTime UploadDate { get; set; }

    public int CaseId { get; set; }
    public Case Case { get; set; }
}