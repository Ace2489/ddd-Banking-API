using System.Collections.ObjectModel;
using Application.IRepository;
using Application.Shared;
using Domain.Entities;
using Domain.Shared;
using MediatR;

namespace Application.Features.History;

public class HistoryCommandHandler(IAccountRepository repository) : IRequestHandler<HistoryCommand, Result<IReadOnlyCollection<Transaction>>>
{
    private readonly IAccountRepository repository = repository;

    public async Task<Result<IReadOnlyCollection<Transaction>>> Handle(HistoryCommand request, CancellationToken cancellationToken)
    {
        Account? account = await repository.GetAsync(request.AccountId, cancellationToken);
        if (account is null) return ApplicationErrors.AccountNotFoundError;
        return Result<IReadOnlyCollection<Transaction>>.Success(account.History(request.Period));
    }
}
