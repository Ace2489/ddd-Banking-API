using Domain.Entities;
using Domain.Shared;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.History;

public record HistoryCommand : IRequest<Result<IReadOnlyCollection<Transaction>>>
{
    public DateTimePeriod Period { get; }

    public Guid AccountId { get; }

    private HistoryCommand(DateTimePeriod period, Guid accountId)
    {
        Period = period;
        AccountId = accountId;
    }

    public static Result<HistoryCommand> Create(DateTimeOffset start, DateTimeOffset end, Guid accountId)
    {
        Result<DateTimePeriod> result = DateTimePeriod.Create(start, end);
        if (result.IsFailure) return result.Error!;
        return new HistoryCommand(result.Value!, accountId);
    }
}