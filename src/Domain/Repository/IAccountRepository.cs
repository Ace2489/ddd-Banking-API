using Domain.Entities;

namespace Domain.Repository;

public interface IAccountRepository
{
    public Task<Account?> Get(Guid id, CancellationToken cancellationToken = default);
}
