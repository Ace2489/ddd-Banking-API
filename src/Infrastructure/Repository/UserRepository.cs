using Application.IRepository;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Context;

namespace Infrastructure.Repository;

public class UserRepository(AppDbContext context) : IUserRepository
{
    private readonly AppDbContext context = context;

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await context.AddAsync(user, cancellationToken);
    }

    public Task<User?> FindByEmail(Email email, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(context.Users.Where(u => u.Email == email).FirstOrDefault());
    }

    public Task<User?> GetAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
