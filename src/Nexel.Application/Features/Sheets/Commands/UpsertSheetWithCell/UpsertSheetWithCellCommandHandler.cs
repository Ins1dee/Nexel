using System.Globalization;
using Nexel.Application.Abstractions;
using Nexel.Application.Abstractions.Messaging;
using Nexel.Application.Features.Cells;
using Nexel.Domain.Modules.Cells.ValueObjects;
using Nexel.Domain.Modules.Sheets;
using Nexel.Domain.Modules.Sheets.ValueObjects;
using Nexel.Domain.Shared;

namespace Nexel.Application.Features.Sheets.Commands.UpsertSheetWithCell;

public sealed class UpsertSheetWithCellCommandHandler : ICommandHandler<UpsertSheetWithCellCommand, CellResponse>
{
    private readonly ISheetRepository _sheetRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpsertSheetWithCellCommandHandler(IUnitOfWork unitOfWork, ISheetRepository sheetRepository)
    {
        _unitOfWork = unitOfWork;
        _sheetRepository = sheetRepository;
    }

    public async Task<Result<CellResponse>> Handle(UpsertSheetWithCellCommand request,
        CancellationToken cancellationToken)
    {
        var sheet = await _sheetRepository.GetByIdAsync(new SheetId(request.SheetId.ToLowerInvariant()));

        if (sheet is null)
        {
            sheet = new Sheet(new SheetId(request.SheetId));

            await _sheetRepository.AddAsync(sheet);
        }


        var upsertResult = sheet.UpsertCell(new CellId(request.CellId), request.Expression.Replace(" ", ""));

        if (upsertResult.IsSuccess)
        {
            var successResponse = new CellResponse(
                request.Expression,
                upsertResult.Value.ToString(CultureInfo.InvariantCulture));

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return successResponse;
        }

        var errorResponse = new CellResponse(request.Expression, upsertResult.Error.Message);

        return Result.Failure(errorResponse);
    }
}