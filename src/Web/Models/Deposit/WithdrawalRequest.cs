using Web.Validators;

namespace Web.Models.Deposit;

public record WithdrawalRequest(
    [GreaterThanZero(ErrorMessage = "Withdrawal amount must be positive")] decimal Amount,
    Guid AccountId);