using System.ComponentModel.DataAnnotations;

namespace Web.Models.Auth;

public record RegisterRequest(string FirstName, string LastName, [EmailAddress] string Email, string Password, string Phone, DateTimeOffset DateOfBirth);
