using System.Text.Json;
using Application.Shared;
using Domain.Entities;
using Domain.Repository;
using Domain.Shared;
using MediatR;

namespace Application.Deposit;

public class DepositCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork) : IRequestHandler<DepositCommand, Result<Account>>
{
    private readonly IAccountRepository accountRepository = accountRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<Result<Account>> Handle(DepositCommand request, CancellationToken cancellationToken)
    {
        Account? account = await accountRepository.Get(request.AccountId, cancellationToken);

        if (account is null) return ApplicationErrors.DepositErrors.AccountNotFoundError;

        Result<Account> depositResult = account.Deposit(request.Amount, DateTimeOffset.UtcNow);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return depositResult;
    }
}
