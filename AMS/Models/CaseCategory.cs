using AMS.BaseEntities;

namespace AMS.Models;

public class CaseCategory: BaseEntity
{

    public string CategoryName { get; set; }
    public bool IsActive { get; set; }
}
