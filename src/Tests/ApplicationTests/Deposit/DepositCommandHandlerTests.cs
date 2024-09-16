using Application;
using Application.Deposit;
using Application.Shared;
using Domain.Entities;
using Domain.Enums;
using Domain.Repository;
using Domain.ValueObjects;
using FluentAssertions;
using NSubstitute;
namespace Tests.ApplicationTests.Deposit;

public class DepositCommandHandlerTests
{
    [Fact]
    public async void Handle_WithValidRequest_ShouldSucceed()
    {
        var accountId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var depositAmount = new Money(1000);
        var accountNumber = "12345";

        var account = new Account(accountId, userId, accountNumber, AccountType.Savings, new Money(1000m));
        var depositCommand = new DepositCommand(accountId, depositAmount);
        var accountRepository = Substitute.For<IAccountRepository>();
        accountRepository.Get(accountId).Returns(account);

        var depositCommandHandler = new DepositCommandHandler(accountRepository);
        var result = await depositCommandHandler.Handle(depositCommand, default);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Error.Should().BeNull();
    }
 
    [Fact]
    public async Task Handle_WithNonExistentAccount_ShouldFail()
    {
        var accountId = Guid.NewGuid();
        var repository = Substitute.For<IAccountRepository>();
        var handler = new DepositCommandHandler(repository);
        repository.Get(accountId).Returns((Account)null!);
        var result = await handler.Handle(new DepositCommand(accountId, new Money(20m)), default);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();

        result.Value.Should().BeNull();
        result.Error!.Code.Should().Be(ApplicationErrors.DepositErrors.AccountNotFoundError.Code);
    }
}
