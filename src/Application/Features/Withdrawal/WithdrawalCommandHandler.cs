using Application.IRepository;
using Application.Shared;
using Domain.Entities;
using Domain.Shared;
using MediatR;

namespace Application.Features.Withdrawal;

public class WithdrawalCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork) : IRequestHandler<WithdrawalCommand, Result<Account>>
{
    private readonly IAccountRepository accountRepository = accountRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<Result<Account>> Handle(WithdrawalCommand request, CancellationToken cancellationToken)
    {
        Account? account = await accountRepository.GetWithTransactionsAsync(request.AccountId, cancellationToken);

        if (account is null) return ApplicationErrors.AccountNotFoundError;

        if (account.OwnerId != request.UserId) return ApplicationErrors.UserNotAccountOwnerError;

        Result<Account> withdrawalResult = account.Withdraw(request.Amount, DateTimeOffset.UtcNow);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return withdrawalResult;
    }

}
