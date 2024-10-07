using Application.IRepository;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Context;

namespace Infrastructure.Repository;

public class UserRepository(AppDbContext context) : IUserRepository
{
    private readonly AppDbContext context = context;

    public Task<User?> AddAsync(User user, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
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
