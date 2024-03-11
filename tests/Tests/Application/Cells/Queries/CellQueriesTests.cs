using Moq;
using Nexel.Application.Features.Cells;
using Nexel.Application.Features.Cells.Queries;
using Nexel.Domain.Errors;
using Nexel.Domain.Modules.Cells.ValueObjects;
using Nexel.Domain.Modules.Sheets;
using Nexel.Domain.Modules.Sheets.ValueObjects;

namespace Tests.Application.Cells.Queries;

public class CellQueriesTests
{
    [Fact]
    public async Task Handle_ValidRequest_Should_ReturnCellResponse()
    {
        // Arrange
        var sheetRepositoryMock = new Mock<ISheetRepository>();

        var sheetId = "validSheetId";
        var cellId = "validCellId";
        var expression = "=3+2";

        var sheet = new Sheet(new SheetId(sheetId));

        sheet.UpsertCell(new CellId(cellId), expression);

        sheetRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<SheetId>()))
            .ReturnsAsync(sheet);

        var query = new GetCellByIdQuery(sheetId, cellId);
        var handler = new GetCellByIdQueryHandler(sheetRepositoryMock.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.IsType<CellResponse>(result.Value);
        var cellResponse = result.Value;
        Assert.Equal(expression, cellResponse.Value);
    }

    [Fact]
    public async Task Handle_SheetNotFound_Should_ReturnFailureResult()
    {
        // Arrange
        var sheetRepositoryMock = new Mock<ISheetRepository>();

        sheetRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<SheetId>()))
            .ReturnsAsync((Sheet)null!);

        var query = new GetCellByIdQuery("nonexistentSheetId", "validCellId");
        var handler = new GetCellByIdQueryHandler(sheetRepositoryMock.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Sheet.NotFound(query.SheetId).Message, result.Error.Message);
    }

    [Fact]
    public async Task Handle_CellNotFound_Should_ReturnFailureResult()
    {
        // Arrange
        var sheetRepositoryMock = new Mock<ISheetRepository>();

        sheetRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<SheetId>()))
            .ReturnsAsync(new Sheet(new SheetId("validSheetId")));

        var query = new GetCellByIdQuery("validSheetId", "nonexistentCellId");
        var handler = new GetCellByIdQueryHandler(sheetRepositoryMock.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Cell.NotFound(query.CellId).Message, result.Error.Message);
    }
}