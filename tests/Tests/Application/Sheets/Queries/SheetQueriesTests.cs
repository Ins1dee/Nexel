using Moq;
using Nexel.Application.Features.Cells;
using Nexel.Application.Features.Sheets.Queries;
using Nexel.Domain.Errors;
using Nexel.Domain.Modules.Cells.ValueObjects;
using Nexel.Domain.Modules.Sheets;
using Nexel.Domain.Modules.Sheets.ValueObjects;
using Xunit.Abstractions;

namespace Tests.Application.Sheets.Queries;

public class SheetQueriesTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public SheetQueriesTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task Handle_ValidRequest_Should_ReturnDictionaryOfCellResponses()
    {
        // Arrange
        var sheetRepositoryMock = new Mock<ISheetRepository>();

        var sheetId = "validSheetId";
        var cellId1 = "cellid1";
        var cellId2 = "cellid2";

        var sheet = new Sheet(new SheetId(sheetId));
        sheet.UpsertCell(new CellId(cellId1), "=3+2");
        sheet.UpsertCell(new CellId(cellId2), "=3+3");

        sheetRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<SheetId>()))
            .ReturnsAsync(sheet);

        var query = new GetSheetByIdQuery(sheetId);
        var handler = new GetSheetByIdQueryHandler(sheetRepositoryMock.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.IsType<Dictionary<string, CellResponse>>(result.Value);
        var sheetResponse = result.Value;

        Assert.Equal(2, sheetResponse.Count);

        Assert.True(sheetResponse.ContainsKey(cellId1));
        Assert.True(sheetResponse.ContainsKey(cellId2));

        Assert.Equal("=3+2", sheetResponse[cellId1].Value);
        Assert.Equal("5", sheetResponse[cellId1].Result);

        Assert.Equal("=3+3", sheetResponse[cellId2].Value);
        Assert.Equal("6", sheetResponse[cellId2].Result);
    }

    [Fact]
    public async Task Handle_SheetNotFound_ShouldReturnFailureResult()
    {
        // Arrange
        var sheetRepositoryMock = new Mock<ISheetRepository>();

        sheetRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<SheetId>()))
            .ReturnsAsync((Sheet)null!);

        var query = new GetSheetByIdQuery("nonexistentSheetId");
        var handler = new GetSheetByIdQueryHandler(sheetRepositoryMock.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Sheet.NotFound(query.SheetId).Message, result.Error.Message);
    }
}