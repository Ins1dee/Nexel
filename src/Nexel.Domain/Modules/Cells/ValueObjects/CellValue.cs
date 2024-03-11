using Nexel.Domain.Modules.Sheets;
using Nexel.Domain.Shared;
using Nexel.Domain.Utilities;

namespace Nexel.Domain.Modules.Cells.ValueObjects;

public record CellValue
{
    private CellValue(string value, double resultValue)
    {
        Value = value;
        ResultValue = resultValue;
    }

    public string Value { get; private set; }
    public double ResultValue { get; private set; }

    public static Result<CellValue> Create(string expression, Sheet sheet)
    {
        var computedExpression = ExpressionHandler.ComputeExpression(expression, sheet);

        if (computedExpression is null) return Result.Failure<CellValue>(Error.DefaultError);

        try
        {
            var result = RpnMathCalculator.Evaluate(computedExpression);

            return new CellValue(expression, result);
        }
        catch (Exception exception)
        {
            return Result
                .Failure<CellValue>(Error.DefaultError);
        }
    }
}