using Application.Authentication;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Models.Auth;

namespace Web.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(ILogger<AuthController> logger, ISender sender) : ControllerBase
{
    private readonly ILogger<AuthController> logger = logger;
    private readonly ISender sender = sender;

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterRequest request)
    {
        Result<RegisterUserCommand> commandResult = RegisterUserCommand.Create(request.FirstName, request.LastName, request.Email, request.Password, request.Phone, request.DateOfBirth);
        if (commandResult.IsFailure) return UnprocessableEntity(commandResult.Error!);
        Result<RegistrationResponse> userResult = await sender.Send(commandResult.Value!);
        
        return Created("api/v1/auth/me", userResult.Value);
    }
}

