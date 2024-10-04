using Application.Features.Deposit;
using Application.Features.Withdrawal;
using Domain.Entities;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Models.Deposit;

namespace Web.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BankController(ISender sender) : ControllerBase
{
    private readonly ISender sender = sender;

    [Authorize]
    [HttpPost("deposit")]
    public async Task<ActionResult> Deposit([FromBody] DepositRequest request, CancellationToken cancellationToken)
    {

        Result<DepositCommand> commandResult = DepositCommand.Create(request.AccountId, request.Amount);

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
        Result<WithdrawalCommand> commandResult = WithdrawalCommand.Create(request.AccountId, request.Amount);

        if (commandResult.Value is null) return UnprocessableEntity(commandResult.Error);
                    
        Result<Account> withdrawalResult = await sender.Send(commandResult.Value, cancellationToken);

        if (withdrawalResult.IsSuccess) return Ok(withdrawalResult.Value);

        return UnprocessableEntity(withdrawalResult.Error);
    }

}
