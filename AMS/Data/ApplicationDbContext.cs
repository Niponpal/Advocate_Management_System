using Microsoft.EntityFrameworkCore;

namespace AMS.Data;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
   
}
