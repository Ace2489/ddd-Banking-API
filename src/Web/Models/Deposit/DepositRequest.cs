using Web.Validators;

namespace Web.Models.Deposit;

public record DepositRequest(
    [GreaterThanZero(ErrorMessage = "Deposit amount must be positive")] decimal Amount,
    Guid AccountId);

