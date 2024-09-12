using Domain.Entities;
using Domain.Shared;
using Domain.ValueObjects;
using MediatR;

namespace Application;

public record DepositCommand(Guid AccountId, Money Amount): IRequest<Result<Account>>;
