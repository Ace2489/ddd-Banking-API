using Domain.Entities;
using Infrastructure.Context;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext context = context;

    [HttpPost("register")]
    public Task<User> Register([FromBody] RegisterRequest request)
    {
        throw new NotImplementedException();
    }
}

