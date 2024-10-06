using Application;
using Application.Features.Withdrawal;
using Application.Shared;
using Domain.Entities;
using Domain.Enums;
using Domain.Repository;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReceivedExtensions;

namespace Tests.ApplicationTests.Withdrawal;

public class WithdrawalCommandHandlerTests
{
    [Fact]
    public async void Handle_WithValidRequest_ShouldSucceed()
    {
        //arrange
        var accountId = Guid.NewGuid();
        var withdrawalAmount = Money.Create(1000).Value!;
        var initialBalance = withdrawalAmount + Money.Create(1000).Value!;
        var userId = Guid.NewGuid();

        var account = Account.CreateWithInitialBalance(accountId, userId, initialBalance, AccountType.Savings).Value!;

        var command = WithdrawalCommand.Create(accountId, withdrawalAmount.Value).Value!;
        var repository = Substitute.For<IAccountRepository>();
        repository.GetAsync(accountId).Returns(account);
        var unitOfWork = Substitute.For<IUnitOfWork>();
        unitOfWork.SaveChangesAsync().Returns(1);

        var commandHandler = new WithdrawalCommandHandler(repository, unitOfWork);

        //act
        var result = await commandHandler.Handle(command, default);

        //assert    
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(account);
        await repository.Received().GetAsync(accountId);
        await unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async void Handle_WithNonExistentAccount_ShouldFail()
    {

        var accountId = Guid.NewGuid();
        var repository = Substitute.For<IAccountRepository>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        var handler = new WithdrawalCommandHandler(repository, unitOfWork);
        repository.GetAsync(accountId).Returns((Account)null!);

        var result = await handler.Handle(WithdrawalCommand.Create(accountId, 1000).Value!, default);

        result.IsFailure.Should().BeTrue();
        result.Error!.Should().Be(ApplicationErrors.AccountNotFoundError);
        await unitOfWork.DidNotReceive().SaveChangesAsync();
    }
}
