using AMS.Models;

public class Advocate
{
public int Id { get; set; }
public string AdvocateName { get; set; }
public string LicenseNumber { get; set; }
public string Specialization { get; set; }
public string Email { get; set; }
public string Phone { get; set; }
public string Address { get; set; }
public int ExperienceYears { get; set; }

public ICollection<Case> Cases { get; set; }
}