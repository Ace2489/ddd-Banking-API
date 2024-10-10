using System.Text.Json;
using Application.IRepository;
using Application.Shared;
using Application.Shared.Models;
using Domain.Entities;
using Domain.Shared;
using MediatR;

namespace Application.Features.History;

public class HistoryCommandHandler(IAccountRepository repository) : IRequestHandler<HistoryCommand, Result<IEnumerable<TransactionResponse>>>
{
    private readonly IAccountRepository repository = repository;

    public async Task<Result<IEnumerable<TransactionResponse>>> Handle(HistoryCommand request, CancellationToken cancellationToken)
    {
        Account? account = await repository.GetAsync(request.AccountId, cancellationToken);
        if (account is null) return ApplicationErrors.AccountNotFoundError;
        var transactionResponses = account.History(request.Period).Select(t => (TransactionResponse)t);
        return Result<IEnumerable<TransactionResponse>>.Success(transactionResponses);
    }
}
