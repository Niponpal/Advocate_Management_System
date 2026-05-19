public class Appointment
{
    public int Id { get; set; }

    public DateTime AppointmentDate { get; set; }
    public string Purpose { get; set; }
    public string Status { get; set; }

    public int ClientId { get; set; }
    public Client Client { get; set; }

    public int AdvocateId { get; set; }
    public Advocate Advocate { get; set; }
}