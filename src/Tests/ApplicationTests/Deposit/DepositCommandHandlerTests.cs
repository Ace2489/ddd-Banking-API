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
        var depositAmount = Money.Create(1000m).Value!;
        var accountNumber = "12345";

        var account = new Account(accountId, userId, accountNumber, AccountType.Savings, Money.Create(1000m).Value!);
        DepositCommand depositCommand = DepositCommand.Create(accountId, depositAmount);
        var accountRepository = Substitute.For<IAccountRepository>();
        accountRepository.GetAsync(accountId).Returns(account);
        var unitOfWork = Substitute.For<IUnitOfWork>();
        unitOfWork.SaveChangesAsync().Returns(1);

        var depositCommandHandler = new DepositCommandHandler(accountRepository, unitOfWork);
        var result = await depositCommandHandler.Handle(depositCommand, default);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Error.Should().BeNull();
        await unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_WithNonExistentAccount_ShouldFail()
    {
        var accountId = Guid.NewGuid();
        var repository = Substitute.For<IAccountRepository>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        var handler = new DepositCommandHandler(repository, unitOfWork);
        repository.GetAsync(accountId).Returns((Account)null!);
        var result = await handler.Handle(DepositCommand.Create(accountId, Money.Create(1000m).Value!), default);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();

        result.Value.Should().BeNull();
        result.Error!.Code.Should().Be(ApplicationErrors.DepositErrors.AccountNotFoundError.Code);
        await unitOfWork.DidNotReceive().SaveChangesAsync();
    }
}
