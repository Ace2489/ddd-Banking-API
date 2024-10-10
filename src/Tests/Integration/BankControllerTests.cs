using System.Net.Http.Json;
using System.Text.Json;
using Application.Shared.Models;
using Domain.Entities;
using Domain.ValueObjects.Name;
using FluentAssertions;
using Infrastructure.Context;
using Microsoft.Extensions.DependencyInjection;
using Tests.Integration.Setup;
using Web.Models;
namespace Tests.Integration;

public class BankControllerTests
{
    private static User GetUser()
    {
        Guid userId = Guid.NewGuid();
        var firstName = FirstName.Create("first").Value!;
        var lastName = LastName.Create("last").Value!;
        var email = Email.Create("email@email").Value!;
        string passwordHash = "Hashed password lol";
        string phone = "phone";
        return User.Create(userId, firstName, lastName, email, phone, DateTimeOffset.UtcNow, passwordHash).Value!;
    }
    [Fact]
    public async void History_WithValidDates_ShouldReturnTransactions()
    {
        BankAppFactory application = new();
        User user = GetUser();
        Account account = user.Accounts.First();
        Money initialBalance = Money.Create(1000).Value!;
        DateTimeOffset timestamp = DateTimeOffset.UtcNow;
        account.Deposit(initialBalance, timestamp);

        IServiceScope serviceScope = application.Services.CreateScope();
        AppDbContext context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        HistoryRequest request = new(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.Now.AddDays(1), account.Id);

        HttpClient client = application.CreateClient();
        HttpResponseMessage res = await client.PostAsJsonAsync("/api/v1/bank/history", request);

        res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var transactions = await res.Content.ReadFromJsonAsync<IEnumerable<TransactionResponse>>();

        transactions.Should().NotBeNull();
        TransactionResponse transaction = transactions!.Single();
        transaction.AccountId.Should().Be(account.Id);
        transaction.Amount.Should().Be(initialBalance.Value);
        transaction.Timestamp.Should().BeCloseTo(timestamp, TimeSpan.FromMinutes(1));
    }
}
