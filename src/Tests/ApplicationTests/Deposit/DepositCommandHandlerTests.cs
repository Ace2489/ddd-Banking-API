using Application;
using Application.Features.Deposit;
using Application.IRepository;
using Application.Shared;
using Domain.Entities;
using Domain.Enums;

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
        var depositAmount = 1000;

        var account = Account.Create(accountId, userId, AccountType.Savings).Value!;
        DepositCommand depositCommand = DepositCommand.Create(accountId, depositAmount, userId).Value!;
        var accountRepository = Substitute.For<IAccountRepository>();
        accountRepository.GetAsync(accountId).Returns(account);
        var unitOfWork = Substitute.For<IUnitOfWork>();
        unitOfWork.SaveChangesAsync().Returns(1);

        var depositCommandHandler = new DepositCommandHandler(accountRepository, unitOfWork);
        var result = await depositCommandHandler.Handle(depositCommand, default);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(account);
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
        var userId = Guid.NewGuid();
        var depositAmount = 1000;
        repository.GetAsync(accountId).Returns((Account)null!);

        var result = await handler.Handle(DepositCommand.Create(accountId, depositAmount, userId).Value!, default);

        result.IsSuccess.Should().BeFalse();
        result.Error!.Should().Be(ApplicationErrors.AccountNotFoundError);
        await unitOfWork.DidNotReceive().SaveChangesAsync();
    }

    [Fact]
    public async void Handle_WithUnownedAccount_ShouldFail()
    {
        //arrange
        var accountId = Guid.NewGuid();
        var withdrawalAmount = Money.Create(1000).Value!;
        var initialBalance = withdrawalAmount + Money.Create(1000).Value!;
        var userId = Guid.NewGuid();

        var account = Account.CreateWithInitialBalance(accountId, Guid.NewGuid(), initialBalance, DateTimeOffset.UtcNow, AccountType.Savings).Value!;

        var command = DepositCommand.Create(accountId, withdrawalAmount.Value, userId).Value!;
        var repository = Substitute.For<IAccountRepository>();
        repository.GetAsync(accountId).Returns(account);
        var unitOfWork = Substitute.For<IUnitOfWork>();
        unitOfWork.SaveChangesAsync().Returns(1);

        var commandHandler = new DepositCommandHandler(repository, unitOfWork);

        //act
        var result = await commandHandler.Handle(command, default);

        //assert    
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ApplicationErrors.UserNotAccountOwnerError);
        await repository.Received().GetAsync(accountId);
    }
}
