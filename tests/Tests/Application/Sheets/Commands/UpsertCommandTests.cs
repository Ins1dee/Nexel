using System.Globalization;
using Moq;
using Nexel.Application.Abstractions;
using Nexel.Application.Features.Cells;
using Nexel.Application.Features.Sheets.Commands.UpsertSheetWithCell;
using Nexel.Domain.Modules.Cells.ValueObjects;
using Nexel.Domain.Modules.Sheets;
using Nexel.Domain.Modules.Sheets.ValueObjects;

namespace Tests.Application.Sheets.Commands;

public class UpsertCommandTests
{
    [Fact]
    public async Task Handle_ValidRequest_Should_ReturnCellResponse()
    {
        // Arrange
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var sheetRepositoryMock = new Mock<ISheetRepository>();

        var sheetId = "validSheetid";
        var cellId = "cellid";
        var expression = "=3+2";

        var sheet = new Sheet(new SheetId(sheetId));
        sheetRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<SheetId>()))
            .ReturnsAsync(sheet);

        sheetRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Sheet>()))
            .Callback<Sheet>(newSheet => sheet = newSheet)
            .Returns(Task.CompletedTask);

        var upsertResult = sheet.UpsertCell(new CellId(cellId), expression.Replace(" ", ""));

        var command = new UpsertSheetWithCellCommand(sheetId, cellId, expression);
        var handler = new UpsertSheetWithCellCommandHandler(unitOfWorkMock.Object, sheetRepositoryMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(upsertResult.IsSuccess);
        Assert.True(result.IsSuccess);
        Assert.IsType<CellResponse>(result.Value);
        var cellResponse = result.Value;

        Assert.Equal(expression, cellResponse.Value);
        Assert.Equal(upsertResult.Value.ToString(CultureInfo.InvariantCulture), cellResponse.Result);

        unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_SheetNotFound_Should_CreateNewSheetAndReturnCellResponse()
    {
        // Arrange
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var sheetRepositoryMock = new Mock<ISheetRepository>();

        var sheetId = "nonexistentSheetId";
        var cellId = "cellId";
        var expression = "=3+2";

        Sheet? sheet = null;

        sheetRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<SheetId>()))
            .ReturnsAsync((Sheet)null!);

        sheetRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Sheet>()))
            .Callback<Sheet>(newSheet => sheet = newSheet)
            .Returns(Task.CompletedTask);

        var upsertResult = sheet?.UpsertCell(new CellId(cellId), expression.Replace(" ", ""));

        var command = new UpsertSheetWithCellCommand(sheetId, cellId, expression);
        var handler = new UpsertSheetWithCellCommandHandler(unitOfWorkMock.Object, sheetRepositoryMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(sheet);
        Assert.True(result.IsSuccess);
        Assert.IsType<CellResponse>(result.Value);
        var cellResponse = result.Value;

        Assert.Equal(expression, cellResponse.Value);
        Assert.Equal("5", cellResponse.Result);

        unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_UpsertFailed_Should_ReturnFailureResult()
    {
        // Arrange
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var sheetRepositoryMock = new Mock<ISheetRepository>();

        var sheetId = "validSheetId";
        var cellId = "cellId";
        var expression = "invalidExpression";

        var sheet = new Sheet(new SheetId(sheetId));
        sheetRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<SheetId>()))
            .ReturnsAsync(sheet);

        sheet.UpsertCell(new CellId(cellId), expression.Replace(" ", ""));

        var command = new UpsertSheetWithCellCommand(sheetId, cellId, expression);
        var handler = new UpsertSheetWithCellCommandHandler(unitOfWorkMock.Object, sheetRepositoryMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
    }
}