using System.Text;
using Nexel.Domain.Shared;

namespace Nexel.Domain.Utilities;

public static class RpnMathCalculator
{
    private static readonly Dictionary<string, int> Precedence = new()
    {
        { "+", 1 }, { "-", 1 },
        { "*", 2 }, { "/", 2 },
        { "^", 3 },
        { "sin", 4 }, { "cos", 4 }, { "sqrt", 4 },
        { "tan", 5 }, { "log", 5 }, { "exp", 5 }, { "abs", 5 }
    };

    private static string ConvertToRpn(string infixExpression)
    {
        var operators = new Stack<string>();
        var outputQueue = new Queue<string>();

        var tokens = TokenizeExpression(infixExpression);

        foreach (var token in tokens)
            if (IsNumber(token))
            {
                outputQueue.Enqueue(token);
            }
            else if (token == "(")
            {
                operators.Push(token);
            }
            else if (token == ")")
            {
                while (operators.Count > 0 && operators.Peek() != "(") outputQueue.Enqueue(operators.Pop());
                if (operators.Count > 0 && operators.Peek() == "(") operators.Pop();
            }
            else if (IsOperator(token) || IsFunction(token))
            {
                while (operators.Count > 0 && (IsOperator(operators.Peek()) || IsFunction(operators.Peek())) &&
                       Precedence[token] <= Precedence[operators.Peek()])
                    outputQueue.Enqueue(operators.Pop());
                operators.Push(token);
            }

        while (operators.Count > 0) outputQueue.Enqueue(operators.Pop());

        return string.Join(" ", outputQueue);
    }

    private static IEnumerable<string> TokenizeExpression(string expression)
    {
        var tokenList = new List<string>();
        var token = new StringBuilder();

        for (var i = 0; i < expression.Length; i++)
        {
            var c = expression[i];

            if (char.IsDigit(c) || c == '.')
            {
                token.Append(c);
            }
            else if (char.IsLetter(c))
            {
                token.Append(c);
            }
            else if (c is '(' or ')' or '+' or '*' or '/' or '^')
            {
                if (token.Length > 0)
                {
                    tokenList.Add(token.ToString());
                    token.Clear();
                }

                tokenList.Add(c.ToString());
            }
            else if (c == '-')
            {
                if (i == 0 || (i > 0 && !char.IsDigit(expression[i - 1]) && expression[i - 1] != ')'))
                {
                    token.Append(c);
                }
                else
                {
                    if (token.Length > 0)
                    {
                        tokenList.Add(token.ToString());
                        token.Clear();
                    }

                    tokenList.Add(c.ToString());
                }
            }
            else
            {
                throw new ArgumentException($"Invalid symbol: {c}");
            }
        }

        if (token.Length > 0) tokenList.Add(token.ToString());

        return tokenList;
    }

    public static double Evaluate(string expression)
    {
        var rpnExpression = ConvertToRpn(expression);
        return EvaluateRpn(rpnExpression);
    }

    private static bool IsOperator(string token)
    {
        return Constants.Operators.Contains(token) && token.Length == 1;
    }

    private static double EvaluateRpn(string rpnExpression)
    {
        var tokens = rpnExpression.Split(' ');
        var stack = new Stack<double>();

        foreach (var token in tokens)
            if (IsNumber(token))
            {
                stack.Push(double.Parse(token));
            }
            else if (IsOperator(token))
            {
                if (stack.Count < 2) throw new ArgumentException($"Insufficient operands for operator: {token}");

                var operand2 = stack.Pop();
                var operand1 = stack.Pop();
                var result = ApplyOperator(token, operand1, operand2);

                stack.Push(result);
            }
            else if (IsFunction(token))
            {
                if (stack.Count < 1) throw new ArgumentException($"Insufficient operands for function: {token}");

                var operand = stack.Pop();
                var result = ApplyFunction(token, operand);
                stack.Push(result);
            }

        if (stack.Count == 1) return stack.Pop();

        throw new ArgumentException("Invalid expression");
    }

    private static bool IsFunction(string token)
    {
        return Constants.Functions.Contains(token);
    }

    private static bool IsNumber(string token)
    {
        return double.TryParse(token, out _);
    }

    private static double ApplyFunction(string func, double operand)
    {
        return func.ToLower() switch
        {
            "sin" => Math.Sin(operand),
            "cos" => Math.Cos(operand),
            "sqrt" => Math.Sqrt(operand),
            "tan" => Math.Tan(operand),
            "log" => Math.Log(operand),
            "exp" => Math.Exp(operand),
            "abs" => Math.Abs(operand),
            _ => throw new ArgumentException($"Invalid function: {func}")
        };
    }

    private static double ApplyOperator(string operation, double operand1, double operand2)
    {
        return operation.ToLower() switch
        {
            "+" => operand1 + operand2,
            "-" => operand1 - operand2,
            "*" => operand1 * operand2,
            "/" => operand2 == 0 ? throw new DivideByZeroException() : operand1 / operand2,
            "^" => Math.Pow(operand1, operand2),

            _ => throw new ArgumentException($"Invalid operation: {operation}")
        };
    }
}