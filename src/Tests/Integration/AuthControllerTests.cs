using System.Net.Http.Json;
using Domain.Entities;
using FluentAssertions;
using Web.Models;
using Web.Models.Auth;

namespace Tests.Integration;

public class AuthControllerTests
{
    [Fact(Skip = "Need other tests first")]
    public async Task Register_WithValidCredentials_ShouldReturnNewUser()
    {
        BankAppFactory application = new();
        string firstname = "First";
        string lastName = "last";
        string email = "email@email";
        string password = "password";

        RegisterRequest registerRequest = new(firstname, lastName, email, password, DateTimeOffset.UtcNow);
        HttpClient client = application.CreateClient();

        HttpResponseMessage res = await client.PostAsJsonAsync("/api/v1/auth/register", registerRequest);

        res.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        Response<User?>? response = await res.Content.ReadFromJsonAsync<Response<User?>>();
        response.Should().NotBeNull();

        User? user = response!.Data!;
        user.FirstName.Should().Be(firstname);
        user.LastName.Should().Be(lastName);
        // user.Email.Should().Be(email);
    }
}
