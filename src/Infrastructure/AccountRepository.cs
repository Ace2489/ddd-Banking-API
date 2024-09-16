using Domain.Entities;
using Domain.Repository;
using Infrastructure.Context;

namespace Infrastructure;

public class AccountRepository(AppDbContext context) : IAccountRepository
{
    private readonly AppDbContext context = context;

    public async Task<Account?> Get(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Set<Account>().FindAsync(id);
    }
}
