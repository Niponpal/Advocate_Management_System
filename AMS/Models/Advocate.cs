using AMS.BaseEntities;
using AMS.Models;

public class Advocate: BaseEntity
{
public string AdvocateName { get; set; }
public string LicenseNumber { get; set; }
public string Specialization { get; set; }
public string Email { get; set; }
public string Phone { get; set; }
public string Address { get; set; }
public long ExperienceYears { get; set; }

public ICollection<Case> Cases { get; set; }
}