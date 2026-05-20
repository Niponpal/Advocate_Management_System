namespace AMS.BaseEntities;

public class BaseEntity<TId>
{
    public TId Id { get; set; }
}
public abstract class BaseEntity : BaseEntity<long> { }
