using Domain.Entities;

namespace Application.Authentication;

public record RegistrationResponse(User User, string AccessToken);