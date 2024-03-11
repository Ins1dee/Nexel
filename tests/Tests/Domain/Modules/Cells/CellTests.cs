using Nexel.Domain.Modules.Cells;
using Nexel.Domain.Modules.Cells.ValueObjects;
using Nexel.Domain.Modules.Sheets;
using Nexel.Domain.Modules.Sheets.ValueObjects;

namespace Tests.Domain.Modules.Cells;

public class CellTests
{
    [Fact]
    public void Create_Should_ReturnFailure_WhenCreateCellValueFailed()
    {
        // Arrange
        var expression = "3 ? 2";
        var sheet = new Sheet(new SheetId("test"));

        // Act
        var createResult = Cell.Create(new CellId("new"), expression, sheet.Id, sheet);

        // Assert
        Assert.True(createResult.IsFailure);
    }

    [Fact]
    public void Create_Should_ReturnCell_WhenCreateCellValueSucceeded()
    {
        // Arrange
        var expression = "=3+2";
        var sheet = new Sheet(new SheetId("test"));

        // Act
        var createResult = Cell.Create(new CellId("new"), expression, sheet.Id, sheet);

        // Assert
        Assert.True(createResult.IsSuccess);
    }

    [Fact]
    public void Update_Should_ReturnCell_WhenCreateCellValueSucceeded()
    {
        // Arrange
        var expression1 = "=3+2";
        var expression2 = "=5+2";
        var sheet = new Sheet(new SheetId("test"));
        var cell = Cell.Create(new CellId("new"), expression1, sheet.Id, sheet);

        // Act
        var createResult = cell.Value.Update(expression2);

        // Assert
        Assert.True(createResult.IsSuccess);
    }

    [Fact]
    public void Update_Should_ReturnFailure_WhenCreateCellValueFailed()
    {
        // Arrange
        var expression1 = "=3+2";
        var expression2 = "5+2";
        var sheet = new Sheet(new SheetId("test"));
        var cell = Cell.Create(new CellId("new"), expression1, sheet.Id, sheet);

        // Act
        var createResult = cell.Value.Update(expression2);

        // Assert
        Assert.True(createResult.IsFailure);
    }
}