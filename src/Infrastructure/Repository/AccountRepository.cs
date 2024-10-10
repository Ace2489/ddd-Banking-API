using Application.IRepository;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class AccountRepository(AppDbContext context) : IAccountRepository
{
    private readonly AppDbContext context = context;

    public async Task<Account?> GetWithTransactionsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Accounts.Include(a => a.Transactions).FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }
}
