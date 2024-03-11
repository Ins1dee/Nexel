namespace Nexel.Domain.Shared;

public static class Constants
{
    public static readonly List<string> Operators = new()
    {
        "+", "-", "*",
        "/", "^"
    };

    public static readonly List<string> Functions = new()
    {
        "sin", "cos", "sqrt",
        "tan", "log", "exp", "abs"
    };
}