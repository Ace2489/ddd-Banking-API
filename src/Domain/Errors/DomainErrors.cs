using Domain.Shared;

namespace Domain.Errors;

public static class DomainErrors
{
    public static class Account
    {
        public static Error InvalidDepositAmountError => new("AccountDepositError", "An invalid deposit amount was given");

    }
    public static class Money
    {
        public static Error NegativeMoneyError => new("MoneyCreationError", "Cannot create negative units of money");

    }
}
