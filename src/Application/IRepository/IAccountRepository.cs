using Domain.Entities;

namespace Application.IRepository;

public interface IAccountRepository
{
    public Task<Account?> GetWithTransactionsAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<Account?> GetAsync(Guid id, CancellationToken cancellationToken = default);
}
