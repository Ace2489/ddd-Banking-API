// using System.Net.Http.Json;
// using Application.Auth.Login;
// using Application.Auth.Register;
// using Application.IRepository;
// using Application.Shared.Models;
// using Domain.Entities;
// using FluentAssertions;
// using Microsoft.Extensions.DependencyInjection;
// using Tests.Integration.Setup;
// using Web.Models.Auth;

// namespace Tests.Integration;


// public class AuthControllerTests
// {
//     [Fact]
//     public async Task Register_WithValidCredentials_ShouldCreateAndReturnANewUser()
//     {
//         BankAppFactory application = new();
//         string firstname = "First";
//         string lastName = "last";
//         string email = "email@email";
//         string password = "password";
//         string phone = "phone";

//         RegisterRequest registerRequest = new(firstname, lastName, email, password, phone, DateTimeOffset.UtcNow);
//         HttpClient client = application.CreateClient();

//         HttpResponseMessage res = await client.PostAsJsonAsync("/api/v1/auth/register", registerRequest);

//         res.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

//         RegistrationResponse? response = await res.Content.ReadFromJsonAsync<RegistrationResponse>();
//         response.Should().NotBeNull();

//         UserResponse user = response!.User;
//         user.FirstName.Should().Be(firstname);
//         user.LastName.Should().Be(lastName);
//         user.Email.Should().Be(email);
//         user.Phone.Should().Be(phone);

//         IServiceScope serviceScope = application.Services.CreateScope();
//         IUserRepository users = serviceScope.ServiceProvider.GetRequiredService<IUserRepository>();

//         User? dbUser = await users.FindByEmailAsync(Email.Create(email).Value!, true, default);

//         dbUser.Should().NotBeNull();
//         dbUser.FirstName.Name.Should().Be(firstname);
//         dbUser.LastName.Name.Should().Be(lastName);
//         dbUser.Email.Mail.Should().Be(email);
//         dbUser.Phone.Should().Be(phone);
//         dbUser.DateOfBirth.Should().BeCloseTo(user.DateOfBirth, TimeSpan.FromMinutes(1));
//     }

//     [Fact]
//     public async Task Login_WithValidCredentials_ShouldReturnUserAndToken()
//     {
//         BankAppFactory application = new();
//         string firstname = "First";
//         string lastName = "last";
//         string email = "email@email";
//         string password = "password";
//         string phone = "phone";

//         RegisterRequest registerRequest = new(firstname, lastName, email, password, phone, DateTimeOffset.UtcNow);
//         HttpClient registrationClient = application.CreateClient();

//         HttpResponseMessage response = await registrationClient.PostAsJsonAsync("/api/v1/auth/register", registerRequest);

//         if (!response.IsSuccessStatusCode) throw new Exception("The user could not be registered");

//         LoginRequest loginRequest = new(email, password);
//         HttpClient client = application.CreateClient();

//         HttpResponseMessage res = await client.PostAsJsonAsync("/api/v1/auth/login", loginRequest);

//         res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

//         LoginResponse? loginResponse = await res.Content.ReadFromJsonAsync<LoginResponse>();
//         loginResponse.Should().NotBeNull();

//         loginResponse!.User.Email.Should().Be(email);
//     }
// }
