using Domain.Entities;
using Domain.ValueObjects;

namespace Application.IRepository;

public interface IUserRepository
{
    public Task<User?> GetAsync(Guid userId, CancellationToken cancellationToken = default);

    public Task<User?> FindByEmailAsync(Email email, bool relatedEntities = false, CancellationToken cancellationToken = default);

    public Task AddAsync(User user, CancellationToken cancellationToken = default);
}
