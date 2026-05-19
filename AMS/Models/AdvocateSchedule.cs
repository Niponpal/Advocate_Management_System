public class AdvocateSchedule
{
    public int Id { get; set; }

    public DateTime ScheduleDate { get; set; }
    public string AvailableTime { get; set; }

    public int AdvocateId { get; set; }
    public Advocate Advocate { get; set; }
}