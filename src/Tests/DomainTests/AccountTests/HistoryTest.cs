using Domain.Entities;
using FluentAssertions;

namespace Tests.DomainTests.AccountTests;

public class HistoryTest
{
    [Fact]
    public void History_WithValidPeriod_ReturnsBoundedTransactions()
    {
        DateTimeOffset start = DateTimeOffset.UtcNow.AddDays(-1);
        DateTimeOffset end = DateTimeOffset.UtcNow.AddDays(1);
        Money initialBalance = Money.Create(1000).Value!;

        DateTimePeriod transactionPeriod = DateTimePeriod.Create(start, end).Value!;
        Account account = Account.CreateWithInitialBalance(Guid.NewGuid(), Guid.NewGuid(), initialBalance, DateTimeOffset.UtcNow).Value!; //this method adds an initial transaction entry

        IReadOnlyCollection<Transaction> transactions = account.History(transactionPeriod);

        Transaction transaction = transactions.Single();
        transactionPeriod.Contains(transaction.Timestamp).Should().BeTrue();
    }
}
