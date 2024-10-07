using Application.IRepository;
using Application.Shared;
using Domain.Entities;
using Domain.Shared;
using MediatR;

namespace Application.Authentication;

public class RegisterUserCommandHandler(IUserRepository userRepository, IAuthenticationService authService) : IRequestHandler<RegisterUserCommand, Result<RegistrationResponse>>
{
    private readonly IUserRepository userRepository = userRepository;
    private readonly IAuthenticationService authService = authService;

    public async Task<Result<RegistrationResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        User? existingUser = await userRepository.FindByEmail(request.Email, cancellationToken);

        if (existingUser is not null) return ApplicationErrors.EmailAlreadyExistsError;

        string passwordHash = await authService.HashPassword(request.Password);

        Result<User> user = User.Create(Guid.NewGuid(), request.FirstName, request.LastName, request.Email, request.Phone, request.DateOfBirth, passwordHash);

        if (user.IsFailure) return user.Error!;

        return new RegistrationResponse(user.Value!, await authService.GenerateTokenAsync(user.Value!.Id, user.Value!.Email));
    }
}
