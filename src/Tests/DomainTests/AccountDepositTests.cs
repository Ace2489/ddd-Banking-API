using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Shared;
using Domain.ValueObjects;

namespace Tests.DomainTests;

public class AccountDepositTests
{
    [Fact]
    public void Deposit_WithNegativeAmount_ReturnsError()
    {
        //arrange
        double negativeDeposit = -1000;
        User testUser = new(Guid.NewGuid(), "first", "last", "email@email", "23456", new DateTime(2003, 12, 10));
        Account account = new(Guid.NewGuid(), testUser.Id, "22345", AccountType.Savings);

        //act
        Result<Account> result = account.Deposit(new Money(negativeDeposit));

        //assert
        Assert.False(result.IsSuccess);
        Assert.Equal(result.Error, DomainErrors.Account.InvalidDepositAmountError);
    }
}