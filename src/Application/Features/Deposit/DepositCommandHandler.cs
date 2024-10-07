using Application.IRepository;
using Application.Shared;
using Domain.Entities;
using Domain.Shared;
using MediatR;

namespace Application.Features.Deposit;

public class DepositCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork) : IRequestHandler<DepositCommand, Result<Account>>
{
    private readonly IAccountRepository accountRepository = accountRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<Result<Account>> Handle(DepositCommand request, CancellationToken cancellationToken)
    {
        Account? account = await accountRepository.GetAsync(request.AccountId, cancellationToken);

        if (account is null) return ApplicationErrors.AccountNotFoundError;

        Result<Account> depositResult = account.Deposit(request.Amount, DateTimeOffset.UtcNow);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return depositResult;
    }
}
