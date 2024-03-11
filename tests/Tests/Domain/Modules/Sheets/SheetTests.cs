using Nexel.Domain.Modules.Cells.ValueObjects;
using Nexel.Domain.Modules.Sheets;
using Nexel.Domain.Modules.Sheets.ValueObjects;

namespace Tests.Domain.Modules.Sheets;

public class SheetTests
{
    [Fact]
    public void UpsertCell_Should_ReturnFailure_WhenCreateCellFailed()
    {
        // Arrange
        var expression = "3+2";
        var sheet = new Sheet(new SheetId("test"));

        // Act
        var upsertResult = sheet.UpsertCell(new CellId("newCell"), expression);

        // Assert
        Assert.True(upsertResult.IsFailure);
    }

    [Fact]
    public void UpsertCell_Should_ReturnFailure_WhenUpdateCellFailed()
    {
        // Arrange
        var expression1 = "=3+2";
        var expression2 = "3+2";
        var sheet = new Sheet(new SheetId("test"));

        // Act
        sheet.UpsertCell(new CellId("newCell"), expression1);
        var upsertResult = sheet.UpsertCell(new CellId("newCell"), expression2);

        // Assert
        Assert.True(upsertResult.IsFailure);
    }

    [Fact]
    public void UpsertCell_Should_ReturnSuccessWithCellValue_WhenUpdateCellSucceeded()
    {
        // Arrange
        var expression1 = "=3+2";
        var expression2 = "=5+8";
        var sheet = new Sheet(new SheetId("test"));

        // Act
        sheet.UpsertCell(new CellId("newCell"), expression1);
        var upsertResult = sheet.UpsertCell(new CellId("newCell"), expression2);

        // Assert
        Assert.True(upsertResult.IsSuccess);
    }
}