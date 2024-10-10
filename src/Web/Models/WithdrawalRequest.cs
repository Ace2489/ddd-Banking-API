using Web.Validators;

namespace Web.Models.Withdrawal;

public record WithdrawalRequest(
    [GreaterThanZero(ErrorMessage = "Withdrawal amount must be positive")] decimal Amount,
    Guid AccountId);