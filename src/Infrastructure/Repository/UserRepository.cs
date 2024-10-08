using Application.IRepository;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class UserRepository(AppDbContext context) : IUserRepository
{
    private readonly AppDbContext context = context;

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await context.AddAsync(user, cancellationToken);
    }

    public Task<User?> FindByEmail(Email email, CancellationToken cancellationToken = default, bool relatedEntities = false)
    {
        User? user = relatedEntities 
            ? context.Users.Where(u => u.Email == email).Include(u=>u.Accounts).FirstOrDefault()
            : context.Users.Where(u => u.Email == email).FirstOrDefault(); 
        return Task.FromResult(user);
    }

    public Task<User?> GetAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
