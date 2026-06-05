using AMS.BaseEntities;

namespace AMS.Models;
public class Appointment : BaseEntity
{

    public DateTime AppointmentDate { get; set; }
    public string Purpose { get; set; }
    public string Status { get; set; }

    public long ClientId { get; set; }
    public Client Client { get; set; }

    public long AdvocateId { get; set; }
    public Advocate Advocate { get; set; }

}