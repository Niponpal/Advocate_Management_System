using AMS.BaseEntities;

public class AdvocateSchedule : BaseEntity
{
   

    public DateTime ScheduleDate { get; set; }
    public string AvailableTime { get; set; }

    public int AdvocateId { get; set; }
    public Advocate Advocate { get; set; }
}