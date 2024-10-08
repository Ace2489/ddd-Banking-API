
using Application.Shared.Models;

namespace Application.Auth.Register;

public record RegistrationResponse(UserResponse User, string AccessToken);
