using System.ComponentModel.DataAnnotations;

namespace Web.Validators;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class GreaterThanZeroAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is null)
            return false;

        if (value is int intValue)
            return intValue >= 0;

        if (value is long longValue)
            return longValue >= 0;

        if (value is float floatValue)
            return floatValue >= 0;

        if (value is double doubleValue)
            return doubleValue >= 0;

        if (value is decimal decimalValue)
            return decimalValue >= 0;

        return false;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} must be greater than zero.";
    }
}
