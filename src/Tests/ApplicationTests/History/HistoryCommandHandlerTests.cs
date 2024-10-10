using Application.Features.History;
using Application.IRepository;
using Application.Shared;
using Application.Shared.Models;
using Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace Tests.ApplicationTests.History;

public class HistoryCommandHandlerTests
{
    private static class Data
    {
        public static DateTimeOffset Start => DateTimeOffset.UtcNow;
        public static DateTimeOffset ContainedPoint => DateTimeOffset.UtcNow.AddDays(1);
        public static DateTimeOffset End => DateTimeOffset.UtcNow.AddDays(2);
    }
    [Fact]
    public async void Handle_WithValidParameters_ShouldReturnTransactions()
    {
        Guid accountId = Guid.NewGuid();
        Guid ownerId = Guid.NewGuid();
        Money initialBalance = Money.Create(1000).Value!;
        Account account = Account.CreateWithInitialBalance(accountId, ownerId, initialBalance, Data.ContainedPoint).Value!; //Adds a transaction entry for the initial balance

        HistoryCommand command = HistoryCommand.Create(Data.Start, Data.End, accountId).Value!;
        IAccountRepository accountRepository = Substitute.For<IAccountRepository>();
        accountRepository.GetWithTransactionsAsync(accountId).Returns(account);

        HistoryCommandHandler commandHandler = new(accountRepository);
        Result<IEnumerable<TransactionResponse>> historyResult = await commandHandler.Handle(command, default);
        historyResult.IsSuccess.Should().BeTrue();
        TransactionResponse tr = historyResult.Value!.Single();

        tr.Timestamp.Should().BeOnOrAfter(Data.Start);
        tr.Timestamp.Should().BeOnOrBefore(Data.End);
    }

    [Fact]
    public async void Handle_WithNonExistentAccount_ShouldReturnTransactionList()
    {
        Guid accountId = Guid.NewGuid();
        HistoryCommand command = HistoryCommand.Create(Data.Start, Data.End, accountId).Value!;
        IAccountRepository accountRepository = Substitute.For<IAccountRepository>();
        accountRepository.GetWithTransactionsAsync(accountId).Returns((Account)null!);

        HistoryCommandHandler commandHandler = new(accountRepository);
        Result<IEnumerable<TransactionResponse>> historyResult = await commandHandler.Handle(command, default);

        historyResult.IsFailure.Should().BeTrue();
        historyResult.Error.Should().Be(ApplicationErrors.AccountNotFoundError);
    }

}
