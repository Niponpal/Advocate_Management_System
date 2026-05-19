using AMS.Models;

public class Case
{
public int Id {get; set; }

public string CaseNumber { get; set; }
public string CaseTitle { get; set; }
public string CaseType { get; set; }
public string Description { get; set; }

public DateTime FilingDate { get; set; }
public string Status { get; set; }

public int AdvocateId { get; set; }
public Advocate Advocate {get; set; }

public int ClientId { get; set; }
public Client Client { get; set; }

public ICollection<Hearing> Hearings { get; set; }
public ICollection<Document> Documents { get; set; }
}