namespace AMS.Repository;

public interface IAdvocateRepository
{
    // CRUD operations for Application entity
    Task<IEnumerable<Advocate>> GetAllApplicationsAsync(CancellationToken cancellationToken);
    Task<Advocate?> GetAdvocateByIdAsync(long id, CancellationToken cancellationToken);
    Task<Advocate> AddAdvocateAsync(Advocate advocate, CancellationToken cancellationToken);
    Task<Advocate?> UpdateAdvocateAsync(Advocate advocate, CancellationToken cancellationToken);
    Task<Advocate> DeleteAdvocateAsync(long id, CancellationToken cancellationToken);

    //Task<bool> IsAlreadyAppliedAsync(long jobId, long userId, CancellationToken cancellationToken);
    //Task<List<Job>> GetAppliedJobsByUserAsync(long userId, CancellationToken cancellationToken);
}

public class AdvocateRepository : IAdvocateRepository
{
    public Task<Advocate> AddAdvocateAsync(Advocate advocate, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Advocate> DeleteAdvocateAsync(long id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Advocate?> GetAdvocateByIdAsync(long id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Advocate>> GetAllApplicationsAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Advocate?> UpdateAdvocateAsync(Advocate advocate, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
