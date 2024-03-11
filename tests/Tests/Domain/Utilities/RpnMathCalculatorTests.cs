using Nexel.Domain.Utilities;

namespace Tests.Domain.Utilities;

public class RpnMathCalculatorTests
{
    [Fact]
    public void Evaluate_Should_ReturnResultOfMathFormula()
    {
        // Arrange
        var test1 = "(5+3)-(4*2)+10";
        var test2 = "sin(30)-sin(30)+cos(30)-cos(30)+cos(30)";

        // Act
        var test1Result = RpnMathCalculator.Evaluate(test1);
        var test2Result = RpnMathCalculator.Evaluate(test2);

        // Assert
        Assert.Equal(10, test1Result);
        Assert.Equal(Math.Cos(30), test2Result);
    }
}