public class Hearing
{
public int Id {get; set; }
public DateTime HearingDate {get; set; }
public string Remarks {get; set; }
public string HearingStatus {get; set; }

public int CaseId {get; set; }
public Case Case {get; set; }

public int CourtId {get; set; }
public Court Court {get; set; }
}