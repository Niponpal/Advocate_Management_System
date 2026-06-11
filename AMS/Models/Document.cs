namespace AMS.Models
{
    public class Document
    {
        public string DocumentTitle { get; set; }
        public string FilePath { get; set; }
        public string DocumentType { get; set; }

        public DateTime UploadDate { get; set; }

        public long CaseId { get; set; }
        public Case Case { get; set; }
    }
}
