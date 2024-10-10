using Application.Auth.Login;
using Application.Auth.Register;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Models.Auth;

namespace Web.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(ISender sender) : ControllerBase
{
    private readonly ISender sender = sender;

    [HttpPost("register")]
    [ProducesResponseType(typeof(RegistrationResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult> Register([FromBody] RegisterRequest request)
    {
        Result<RegisterUserCommand> commandResult = RegisterUserCommand.Create(request.FirstName, request.LastName, request.Email, request.Password, request.Phone, request.DateOfBirth);
        if (commandResult.IsFailure) return UnprocessableEntity(commandResult.Error!);
        Result<RegistrationResponse> userResult = await sender.Send(commandResult.Value!);

        return userResult.IsSuccess ? Created("api/v1/auth/me", userResult.Value) : UnprocessableEntity(userResult.Error);

    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult> Login([FromBody] LoginRequest request)
    {
        Result<LoginCommand> commandResult = LoginCommand.Create(request.Email, request.Password);
        if (commandResult.IsFailure) return UnprocessableEntity(commandResult.Error);

        Result<LoginResponse> loginResult = await sender.Send(commandResult.Value!);
        return loginResult.IsSuccess ? Ok(loginResult.Value) : BadRequest(loginResult.Error);
    }
}

