using Application.IRepository;
using Application.Shared;
using Application.Shared.Models;
using Domain.Entities;
using Domain.Shared;
using MediatR;

namespace Application.Features.Info;

public class InfoCommandHandler(IAccountRepository repository) : IRequestHandler<InfoCommand, Result<AccountResponse>>
{
    private readonly IAccountRepository repository = repository;

    public async Task<Result<AccountResponse>> Handle(InfoCommand request, CancellationToken cancellationToken)
    {
        Account? account = await repository.GetAsync(request.AccountId, cancellationToken);
        if (account is null) return ApplicationErrors.AccountNotFoundError;
        if (account.OwnerId != request.UserId) return ApplicationErrors.UserNotAccountOwnerError;
        return (AccountResponse)account;
    }
}
