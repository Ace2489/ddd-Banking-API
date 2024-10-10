using Application.Shared.Models;
using Domain.Shared;
using MediatR;

namespace Application.Features.Info;

public record InfoCommand(Guid AccountId, Guid UserId): IRequest<Result<AccountResponse>>;
