using Application;
using Domain.Entities;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BankController(ISender sender) : ControllerBase
{
    private readonly ISender sender = sender;

    [HttpPost("deposit")]
    public async Task<ActionResult> Deposit([FromBody] DepositCommand command, CancellationToken cancellationToken)
    {
        Result<Account> result = await sender.Send(command, cancellationToken);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return UnprocessableEntity(result.Error);
    }

}
