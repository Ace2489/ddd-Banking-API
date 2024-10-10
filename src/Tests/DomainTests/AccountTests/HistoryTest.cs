using Domain.Entities;
using FluentAssertions;

namespace Tests.DomainTests.AccountTests;

public class HistoryTest
{
    [Fact]
    public void History_WithValidPeriod_ReturnsBoundedTransactions()
    {
        DateTimeOffset start = new(new DateTime(2024, 10, 09));
        DateTimeOffset end = new(new DateTime(2024, 10, 11));
        Money initialBalance = Money.Create(1000).Value!;

        DateTimePeriod transactionPeriod = DateTimePeriod.Create(start, end).Value!;
        Account account = Account.CreateWithInitialBalance(Guid.NewGuid(), Guid.NewGuid(), initialBalance, DateTimeOffset.UtcNow).Value!; //this method adds an initial transaction entry

        IReadOnlyCollection<Transaction> transactions = account.History(transactionPeriod);

        Transaction transaction = transactions.Single();
        transactionPeriod.Contains(transaction.Timestamp).Should().BeTrue();
    }
}
