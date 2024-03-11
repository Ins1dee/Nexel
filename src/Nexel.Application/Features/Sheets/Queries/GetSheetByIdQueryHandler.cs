using System.Globalization;
using Nexel.Application.Abstractions.Messaging;
using Nexel.Application.Features.Cells;
using Nexel.Domain.Errors;
using Nexel.Domain.Modules.Sheets;
using Nexel.Domain.Modules.Sheets.ValueObjects;
using Nexel.Domain.Shared;

namespace Nexel.Application.Features.Sheets.Queries;

public sealed class GetSheetByIdQueryHandler : IQueryHandler<GetSheetByIdQuery, Dictionary<string, CellResponse>>
{
    private readonly ISheetRepository _sheetRepository;

    public GetSheetByIdQueryHandler(ISheetRepository sheetRepository)
    {
        _sheetRepository = sheetRepository;
    }

    public async Task<Result<Dictionary<string, CellResponse>>> Handle(
        GetSheetByIdQuery request,
        CancellationToken cancellationToken)
    {
        var sheet = await _sheetRepository.GetByIdAsync(new SheetId(request.SheetId));

        if (sheet is null)
            return Result.Failure<Dictionary<string, CellResponse>>(DomainErrors.Sheet.NotFound(request.SheetId));

        var sheetResponse = new Dictionary<string, CellResponse>();
        foreach (var cell in sheet.Cells)
        {
            var cellResponse = new CellResponse(
                cell.CellValue.Value,
                cell.CellValue.ResultValue.ToString(CultureInfo.InvariantCulture));

            sheetResponse.Add(cell.Id.Value, cellResponse);
        }

        return sheetResponse;
    }
}