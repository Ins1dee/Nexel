using System.Globalization;
using Nexel.Application.Abstractions.Messaging;
using Nexel.Domain.Errors;
using Nexel.Domain.Modules.Cells.ValueObjects;
using Nexel.Domain.Modules.Sheets;
using Nexel.Domain.Modules.Sheets.ValueObjects;
using Nexel.Domain.Shared;

namespace Nexel.Application.Features.Cells.Queries;

public sealed class GetCellByIdQueryHandler : IQueryHandler<GetCellByIdQuery, CellResponse>
{
    private readonly ISheetRepository _sheetRepository;

    public GetCellByIdQueryHandler(ISheetRepository sheetRepository)
    {
        _sheetRepository = sheetRepository;
    }

    public async Task<Result<CellResponse>> Handle(GetCellByIdQuery request, CancellationToken cancellationToken)
    {
        var sheet = await _sheetRepository.GetByIdAsync(new SheetId(request.SheetId));

        if (sheet is null) return Result.Failure<CellResponse>(DomainErrors.Sheet.NotFound(request.SheetId));

        var cell = sheet.Cells.SingleOrDefault(x => x.Id == new CellId(request.CellId));

        if (cell is null) return Result.Failure<CellResponse>(DomainErrors.Cell.NotFound(request.CellId));

        var cellResponse = new CellResponse(
            cell.CellValue.Value,
            cell.CellValue.ResultValue.ToString(CultureInfo.InvariantCulture));

        return cellResponse;
    }
}