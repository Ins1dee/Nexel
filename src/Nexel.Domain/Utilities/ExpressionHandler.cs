using System.Globalization;
using System.Text.RegularExpressions;
using Nexel.Domain.Modules.Sheets;
using Nexel.Domain.Shared;

namespace Nexel.Domain.Utilities;

public static class ExpressionHandler
{
    public static string? ComputeExpression(string expression, Sheet sheet)
    {
        if (!expression.StartsWith('=') && IsFormula(expression)) return null;

        var variablesFromExpression = ParseVariablesFromExpression(expression);

        if (variablesFromExpression.Count == 0) return expression.Replace("=", "");

        var cellsFromExpression = sheet.Cells
            .Where(cell => variablesFromExpression.Contains(cell.Id.Value))
            .ToList();

        return !variablesFromExpression
            .All(variable => cellsFromExpression
                .Select(cell => cell.Id.Value)
                .Contains(variable))
            ? null
            : cellsFromExpression.OrderByDescending(cell => cell.Id.Value.Length)
                .Aggregate(expression, (current, item) =>
                    current
                        .Replace("=", "")
                        .Replace(" ", "")
                        .Replace(item.Id.Value, item.CellValue.ResultValue.ToString(CultureInfo.InvariantCulture)));
    }

    private static List<string> ParseVariablesFromExpression(string expression)
    {
        const string pattern = @"\b[a-zA-Z_]\w*\b";
        var variableMatches = Regex.Matches(expression, pattern);

        var variables = variableMatches
            .Select(match => match.Value)
            .Where(variable => !Constants.Functions.Contains(variable))
            .Distinct()
            .ToList();

        return variables;
    }

    private static bool IsFormula(string expression)
    {
        return Constants.Functions.Any(expression.Contains)
               || Constants.Operators.Any(expression.Contains);
    }
}