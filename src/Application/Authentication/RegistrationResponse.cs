
using Application.Shared.Models;

namespace Application.Authentication;

public record RegistrationResponse(UserResponse User, string AccessToken);
