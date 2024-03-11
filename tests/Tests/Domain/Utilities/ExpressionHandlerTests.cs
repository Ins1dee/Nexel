using Nexel.Domain.Modules.Cells.ValueObjects;
using Nexel.Domain.Modules.Sheets;
using Nexel.Domain.Modules.Sheets.ValueObjects;
using Nexel.Domain.Utilities;

namespace Tests.Domain.Utilities;

public class ExpressionHandlerTests
{
    [Fact]
    public void ComputeExpression_Should_ReturnNull_WhenNotAllVariablesAreExistingCells()
    {
        // Arrange
        var expression = "=var1 + var2";
        var sheet = new Sheet(new SheetId("test"));

        // Act
        var computeResult = ExpressionHandler.ComputeExpression(expression, sheet);

        // Assert
        Assert.Null(computeResult);
    }

    [Fact]
    public void ComputeExpression_Should_ReturnNull_WhenExpressionIsFormulaWithoutFormulaStartSymbol()
    {
        // Arrange
        var expression = "3+5";
        var sheet = new Sheet(new SheetId("test"));

        // Act
        var computeResult = ExpressionHandler.ComputeExpression(expression, sheet);

        // Assert
        Assert.Null(computeResult);
    }

    [Fact]
    public void ComputeExpression_Should_ReturnDefaultExpression_WhenListOfVariablesIsEmpty()
    {
        // Arrange
        var expression = "=3 + 5";
        var sheet = new Sheet(new SheetId("test"));

        // Act
        var computeResult = ExpressionHandler.ComputeExpression(expression, sheet);

        // Assert
        Assert.Equal(expression.Replace("=", ""), computeResult);
    }

    [Fact]
    public void ComputeExpression_Should_ReturnExpressionReplacedWithActualValues_WhenAllVariablesAreExistingCells()
    {
        // Arrange
        var expression = "=var1+3";
        var test = "5";
        var sheet = new Sheet(new SheetId("test"));
        sheet.UpsertCell(new CellId("var1"), test);

        // Act
        var computeResult = ExpressionHandler.ComputeExpression(expression, sheet);

        // Assert
        Assert.Equal("5+3", computeResult);
    }
}