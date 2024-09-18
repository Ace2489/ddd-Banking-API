using Domain.Entities;

namespace Domain.Repository;

public interface IAccountRepository
{
    public Task<Account?> GetAsync(Guid id, CancellationToken cancellationToken = default);
}
