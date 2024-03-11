using FluentValidation;
using Nexel.Domain.Shared;

namespace Nexel.Application.Extensions;

public static class RuleBuilderExtensions
{
    public static void IsCellOrSheet<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        ruleBuilder
            .NotEmpty()
            .WithMessage("The string cannot be empty.")
            .Must(input => !char.IsDigit(input.FirstOrDefault()))
            .WithMessage("The string cannot start with a number.")
            .Must(input => !Constants.Operators.Any(input.Contains))
            .WithMessage("The string cannot contain operation symbols.")
            .Must(input => !Constants.Functions.Any(input.Contains))
            .WithMessage("The string cannot contain math functions.");
    }
}