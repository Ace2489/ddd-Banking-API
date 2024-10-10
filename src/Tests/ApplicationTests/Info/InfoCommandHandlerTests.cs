using Application.Features.Info;
using Application.IRepository;
using Application.Shared;
using Application.Shared.Models;
using Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace Tests.ApplicationTests.Info;

public class InfoCommandHandlerTests
{
    [Fact]
    public async void Handle_WithValidInput_ShouldSucceed()
    {
        Guid accountId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        Money initialBalance = Money.Create(1000).Value!;
        Account account = Account.CreateWithInitialBalance(accountId, userId, initialBalance, DateTimeOffset.UtcNow).Value!;

        InfoCommand command = new(accountId, userId);
        IAccountRepository repository = Substitute.For<IAccountRepository>();
        repository.GetAsync(accountId).Returns(account);

        InfoCommandHandler commandHandler = new(repository);

        Result<AccountResponse> response = await commandHandler.Handle(command, default);

        response.IsSuccess.Should().BeTrue();
        AccountResponse accountResponse = response.Value!;
        accountResponse.AccountId.Should().Be(account.Id);
    }

    [Fact]
    public async void Handle_WithNonExistentAccount_ShouldFail()
    {
        var accountId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var repository = Substitute.For<IAccountRepository>();
        repository.GetAsync(accountId).Returns((Account)null!);
        var command = new InfoCommand(accountId, userId);

        var handler = new InfoCommandHandler(repository);

        var result = await handler.Handle(command, default);

        result.IsFailure.Should().BeTrue();
        result.Error!.Should().Be(ApplicationErrors.AccountNotFoundError);
    }

    [Fact]
    public async void Handle_WithUnownedAccount_ShouldFail()
    {
        //arrange
        var accountId = Guid.NewGuid();
        var initialBalance = Money.Create(1000).Value!;
        var userId = Guid.NewGuid();

        var account = Account.CreateWithInitialBalance(accountId, Guid.NewGuid(), initialBalance, DateTimeOffset.UtcNow).Value!;

        var command = new InfoCommand(accountId, userId);
        var repository = Substitute.For<IAccountRepository>();
        repository.GetAsync(accountId).Returns(account);

        var commandHandler = new InfoCommandHandler(repository);

        //act
        var result = await commandHandler.Handle(command, default);

        //assert    
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ApplicationErrors.UserNotAccountOwnerError);
    }
}
