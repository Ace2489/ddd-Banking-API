namespace Web.Models.Auth;

public record RegisterResponse(string FirstName, string LastName, string Email, string Phone, string DateOfBirth);