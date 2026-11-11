using System.ComponentModel.DataAnnotations;

namespace Lesson2.Models.DTO;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class PositiveDecimalAttribute : ValidationAttribute
{
    public PositiveDecimalAttribute()
    {
        ErrorMessage = "The products cost must be greater than zero.";
    }

    public override bool IsValid(object? value)
    {
        return value is decimal amount && amount > decimal.Zero;
    }
}
