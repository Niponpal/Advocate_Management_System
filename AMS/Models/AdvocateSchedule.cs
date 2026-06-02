using AMS.BaseEntities;
using AMS.Models;

public class AdvocateSchedule : BaseEntity
{
    public DateTime ScheduleDate { get; set; }
    public string AvailableTime { get; set; }

    public long AdvocateId { get; set; }
    public Advocate Advocate { get; set; }
}