using Domain.Entities;

namespace Application.IRepository;

public interface IAccountRepository
{
    public Task<Account?> GetAsync(Guid id, CancellationToken cancellationToken = default);
}
