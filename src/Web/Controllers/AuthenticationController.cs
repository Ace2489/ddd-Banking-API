using Domain.Entities;
using Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Models.Auth;

namespace Web.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(ILogger<AuthController> logger, IPasswordHasher<User> passwordHasher) : ControllerBase
{
    private readonly IPasswordHasher<User> passwordHasher = passwordHasher;

    [HttpPost("register")]
    public Task<User> Register([FromBody] RegisterRequest request)
    {
        throw new NotImplementedException();
    }
}

