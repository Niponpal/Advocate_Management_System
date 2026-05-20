using AMS.BaseEntities;

public class Appointment :BaseEntity
{

    public DateTime AppointmentDate { get; set; }
    public string Purpose { get; set; }
    public string Status { get; set; }

    public int ClientId { get; set; }
    public Client Client { get; set; }

    public int AdvocateId { get; set; }
    public Advocate Advocate { get; set; }
}