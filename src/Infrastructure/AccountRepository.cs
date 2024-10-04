using Domain.Entities;
using Domain.Repository;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AccountRepository(AppDbContext context) : IAccountRepository
{
    private readonly AppDbContext context = context;

    public async Task<Account?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Accounts.Include(a => a.Transactions).FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }
}
