using Application.Shared.Models;

namespace Application.Auth.Login;

public record LoginResponse(UserResponse User, string AccessToken);
