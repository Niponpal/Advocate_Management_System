namespace AMS.Models;

public class TaskManagement
{
    public string TaskTitle { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; }

    public long AdvocateId { get; set; }
    public Advocate Advocate { get; set; }
}
