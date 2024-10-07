using System.Text.Json;
using Application.IRepository;
using Application.Shared;
using Application.Shared.Models;
using Domain.Entities;
using Domain.Shared;
using MediatR;

namespace Application.Auth.Login;

public class LoginCommandHandler(IUserRepository userRepository, IAuthenticationService authService) : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly IUserRepository userRepository = userRepository;
    private readonly IAuthenticationService authService = authService;

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        User? existingUser = await userRepository.FindByEmail(request.Email, cancellationToken);
        if (existingUser is null) return ApplicationErrors.AccountNotFoundError;
        bool verifiedPassword = await authService.VerifyPassword(existingUser.PasswordHash, request.Password);
        if (verifiedPassword != true) return ApplicationErrors.AccountNotFoundError;

        return new LoginResponse((UserResponse)existingUser, await authService.GenerateTokenAsync(existingUser.Id, existingUser.Email));
    }
}
