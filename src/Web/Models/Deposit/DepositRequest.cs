using Web.Validators;

namespace Web.Models.Deposit;

public record DepositRequest(
    [GreaterThanZero(ErrorMessage = "Deposit amount must not be negative")] decimal Amount,
    Guid AccountId);

