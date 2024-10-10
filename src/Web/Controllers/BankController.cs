using System.Text.Json;
using Application.Features.Deposit;
using Application.Features.History;
using Application.Features.Withdrawal;
using Application.Shared.Models;
using Domain.Entities;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;
using Web.Models;
using Web.Models.Deposit;
using Web.Models.Withdrawal;

namespace Web.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class BankController(ISender sender) : ControllerBase
{
    private readonly ISender sender = sender;

    [HttpPost("deposit")]
    public async Task<ActionResult<Account>> Deposit([FromBody] DepositRequest request, CancellationToken cancellationToken)
    {
        Guid userId = this.GetLoggedInUser();

        Result<DepositCommand> commandResult = DepositCommand.Create(request.AccountId, request.Amount, userId);

        if (commandResult.Value is null) return UnprocessableEntity(commandResult.Error);

        Result<Account> result = await sender.Send(commandResult.Value, cancellationToken);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return UnprocessableEntity(result.Error);
    }

    [HttpPost("withdraw")]
    public async Task<ActionResult<Account>> Withdraw([FromBody] WithdrawalRequest request, CancellationToken cancellationToken)
    {
        Guid userId = this.GetLoggedInUser();

        Result<WithdrawalCommand> commandResult = WithdrawalCommand.Create(request.AccountId, request.Amount, userId);

        if (commandResult.Value is null) return UnprocessableEntity(commandResult.Error);

        Result<Account> withdrawalResult = await sender.Send(commandResult.Value, cancellationToken);

        if (withdrawalResult.IsSuccess) return Ok(withdrawalResult.Value);

        return UnprocessableEntity(withdrawalResult.Error);
    }

    [HttpPost("history")]
    public async Task<ActionResult> History([FromBody] HistoryRequest request, CancellationToken token)
    {
        Result<HistoryCommand> commandResult = HistoryCommand.Create(request.Start, request.End, request.AccountId);

        if (commandResult.Value is null) return UnprocessableEntity(commandResult.Error);

        Result<IEnumerable<TransactionResponse>> historyResult = await sender.Send(commandResult.Value, token);

        if (historyResult.IsSuccess) return Ok(historyResult.Value);

        return UnprocessableEntity(historyResult.Error);
    }
}
