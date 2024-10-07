using Domain.Entities;
using Domain.ValueObjects;

namespace Application.IRepository;

public interface IUserRepository
{
    public Task<User?> GetAsync(Guid userId, CancellationToken cancellationToken = default);

    public Task<User?> FindByEmail(Email email, CancellationToken cancellationToken = default);

    public Task<User?> AddAsync(User user, CancellationToken cancellationToken = default);
}
