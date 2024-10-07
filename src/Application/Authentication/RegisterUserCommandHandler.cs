using Application.IRepository;
using Application.Shared;
using Application.Shared.Models;
using Domain.Entities;
using Domain.Shared;
using MediatR;

namespace Application.Authentication;

public class RegisterUserCommandHandler(IUserRepository userRepository, IAuthenticationService authService, IUnitOfWork unitOfWork) : IRequestHandler<RegisterUserCommand, Result<RegistrationResponse>>
{
    private readonly IUserRepository userRepository = userRepository;
    private readonly IAuthenticationService authService = authService;
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<Result<RegistrationResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        User? existingUser = await userRepository.FindByEmail(request.Email, cancellationToken);

        if (existingUser is not null) return ApplicationErrors.EmailAlreadyExistsError;

        string passwordHash = await authService.HashPassword(request.Password);

        Result<User> user = User.Create(Guid.NewGuid(), request.FirstName, request.LastName, request.Email, request.Phone, request.DateOfBirth, passwordHash);

        if (user.IsFailure) return user.Error!;

        await userRepository.AddAsync(user.Value!, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new RegistrationResponse((UserResponse)user.Value!, await authService.GenerateTokenAsync(user.Value!.Id, user.Value!.Email));
    }
}
