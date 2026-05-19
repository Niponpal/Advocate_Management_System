using AMS.Models;

public class Court
{
public int Id { get; set; }

public string CourtName { get; set; }
public string CourtType { get; set; }
public string Location { get; set; }

public  ICollection<Hearing> Hearings { get; set; }
}