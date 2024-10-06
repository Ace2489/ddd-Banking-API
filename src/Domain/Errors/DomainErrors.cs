using Domain.Shared;

namespace Domain.Errors;

public static class DomainErrors
{
    public static class Account
    {
        public static Error InvalidDepositAmountError => new("Account.DepositError", "An invalid deposit amount was given");
        public static Error InsufficientFundsError => new("Account.InsufficientFundsError", "The requested withdrawal amount is greater than your account balance");

    }
    public static class Money
    {
        public static Error NegativeMoneyError => new("Money.CreationError", "Cannot create negative units of money");
    }

    public static class Name
    {
        public static Error MaxCharacterInputError => new("Name.MaxCharacterError", $"Name cannot be greater than {ValueObjects.BaseName.MaxLength} characters");
        public static Error EmptyInputError => new("Name.EmptyInputError", "Name must be a non-empty string");
    }

    public static class Email
    {
        public static Error EmptyInputError => new("Email.EmptyInputError", "Email must be a non-empty string");

        public static Error MaxCharacterInputError => new("Email.MaxCharacterError", $"The email is too long. Maximum length is {ValueObjects.Email.MaxLength} characters");
    }
}
