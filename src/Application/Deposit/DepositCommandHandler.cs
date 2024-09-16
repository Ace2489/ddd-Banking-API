using System.Text.Json;
using Application.Shared;
using Domain.Entities;
using Domain.Repository;
using Domain.Shared;
using MediatR;

namespace Application.Deposit;

public class DepositCommandHandler(IAccountRepository accountRepository) : IRequestHandler<DepositCommand, Result<Account>>
{
    private readonly IAccountRepository accountRepository = accountRepository;

    public async Task<Result<Account>> Handle(DepositCommand request, CancellationToken cancellationToken)
    {
        Account? account = await accountRepository.Get(request.AccountId, cancellationToken);

        if (account is null) return ApplicationErrors.DepositErrors.AccountNotFoundError;

        return account.Deposit(request.Amount, DateTimeOffset.UtcNow);

    }
}
