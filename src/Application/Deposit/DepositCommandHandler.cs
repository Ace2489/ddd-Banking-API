using System.Text.Json;
using Application.Shared;
using Domain.Entities;
using Domain.Repository;
using Domain.Shared;
using Domain.ValueObjects;
using MediatR;

namespace Application.Deposit;

public class DepositCommandHandler(IAccountRepository accountRepository) : IRequestHandler<DepositCommand, Result<Account>>
{
    private readonly IAccountRepository accountRepository = accountRepository;

    public async Task<Result<Account>> Handle(DepositCommand request, CancellationToken cancellationToken)
    {
        Account? account = await accountRepository.Get(request.AccountId);

        if (account is null) return ApplicationErrors.DepositErrors.AccountNotFoundError;

        account.Deposit(request.Amount, DateTimeOffset.UtcNow);

        return account;
    }
}
