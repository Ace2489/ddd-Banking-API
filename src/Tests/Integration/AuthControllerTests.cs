using System.Net.Http.Json;
using Application.Auth.Register;
using Application.Shared.Models;
using FluentAssertions;
using Web.Models.Auth;

namespace Tests.Integration;

public class AuthControllerTests
{
    // [Fact]
    public async Task Register_WithValidCredentials_ShouldCreateAndReturnANewUser()
    {
        BankAppFactory application = new();
        string firstname = "First";
        string lastName = "last";
        string email = "email@email";
        string password = "password";
        string phone = "phone";

        RegisterRequest registerRequest = new(firstname, lastName, email, password, phone, DateTimeOffset.UtcNow);
        HttpClient client = application.CreateClient();

        HttpResponseMessage res = await client.PostAsJsonAsync("/api/v1/auth/register", registerRequest);

        res.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        RegistrationResponse? response = await res.Content.ReadFromJsonAsync<RegistrationResponse>();
        response.Should().NotBeNull();

        UserResponse user = response!.User;
        user.FirstName.Should().Be(firstname);
        user.LastName.Should().Be(lastName);
        user.Email.Should().Be(email);
        user.Phone.Should().Be(phone);

        Console.WriteLine("THE ACCESS TOKEN IS " + response.AccessToken);
    }
}
