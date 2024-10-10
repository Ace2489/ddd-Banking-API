using System.Net.Http.Json;
using Application.Features.Info;
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
    [Fact]
    public async void History_WithValidDates_ShouldReturnTransactions()
    {
        BankAppFactory application = new();
        User user = GetUser();
        Account account = InitialiseAccount(user);
        await PersistToDB(application, user);

        HistoryRequest request = new(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.Now.AddDays(1), account.Id);

        HttpClient client = application.CreateClient();
        HttpResponseMessage res = await client.PostAsJsonAsync("/api/v1/bank/history", request);

        res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var transactions = await res.Content.ReadFromJsonAsync<IEnumerable<TransactionResponse>>();

        transactions.Should().NotBeNull();
        TransactionResponse transaction = transactions!.Single();
        transaction.AccountId.Should().Be(account.Id);
        transaction.Amount.Should().Be(account.Balance.Value);
        transaction.Timestamp.Should().BeCloseTo(account.Transactions.Single().Timestamp, TimeSpan.FromMinutes(1));
    }
    [Fact]

    public async void Info_WithValidIds_ShouldReturnAccount()
    {
        BankAppFactory application = new();
        User user = GetUser();
        Account account = InitialiseAccount(user);
        await PersistToDB(application, user);

        InfoRequest request = new(account.Id, user.Id);
        HttpClient client = application.CreateClient();
        HttpResponseMessage res = await client.GetAsync($"/api/v1/bank/info?accountId={request.AccountId}&userId={request.UserId}");

        res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        AccountResponse? response = await res.Content.ReadFromJsonAsync<AccountResponse>();
        response.Should().NotBeNull();
        response.Should().Be((AccountResponse)account);
    }

    private static User GetUser()
    {
        Guid userId = Guid.NewGuid();
        FirstName firstName = FirstName.Create("first").Value!;
        LastName lastName = LastName.Create("last").Value!;
        Email email = Email.Create("email@email").Value!;
        string passwordHash = "Hashed password lol";
        string phone = "phone";
        return User.Create(userId, firstName, lastName, email, phone, DateTimeOffset.UtcNow, passwordHash).Value!;
    }

    private static Account InitialiseAccount(User user)
    {
        Account account = user.Accounts.First();
        Money initialBalance = Money.Create(1000).Value!;
        DateTimeOffset timestamp = DateTimeOffset.UtcNow;
        account.Deposit(initialBalance, timestamp);
        return account;
    }
    private static async Task PersistToDB(BankAppFactory app, User user)
    {
        IServiceScope serviceScope = app.Services.CreateScope();
        AppDbContext context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

    }

}
