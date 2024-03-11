using Nexel.Domain.Modules.Cells.ValueObjects;
using Nexel.Domain.Modules.Sheets;
using Nexel.Domain.Modules.Sheets.ValueObjects;

namespace Tests.Domain.Modules.Cells;

public class CellValueTests
{
    [Fact]
    public void Create_Should_ReturnFailure_WhenComputedExpressionIsNull()
    {
        // Arrange
        var expression = "var1 + var2";
        var sheet = new Sheet(new SheetId("test"));

        // Act
        var createResult = CellValue.Create(expression, sheet);

        // Assert
        Assert.True(createResult.IsFailure);
    }

    [Fact]
    public void Create_Should_ReturnFailure_WhenEvaluateMethodThrowException()
    {
        // Arrange
        var expression = "3 ? 2";
        var sheet = new Sheet(new SheetId("test"));

        // Act
        var createResult = CellValue.Create(expression, sheet);

        // Assert
        Assert.True(createResult.IsFailure);
    }

    [Fact]
    public void Create_Should_ReturnCellValue_WhencalculationSucceed()
    {
        // Arrange
        var expression = "=3+2";
        var sheet = new Sheet(new SheetId("test"));

        // Act
        var createResult = CellValue.Create(expression, sheet);

        // Assert
        Assert.False(createResult.IsFailure);
    }
}