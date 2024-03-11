using Nexel.Domain.Abstractions;
using Nexel.Domain.Modules.Cells.ValueObjects;
using Nexel.Domain.Modules.Sheets;
using Nexel.Domain.Modules.Sheets.ValueObjects;
using Nexel.Domain.Shared;

namespace Nexel.Domain.Modules.Cells;

public class Cell : Entity<CellId>
{
    private Cell(CellId id, CellValue cellValue, SheetId sheetId, Sheet sheet) : base(id)
    {
        CellValue = cellValue;
        SheetId = sheetId;
        Sheet = sheet;
    }

    private Cell()
    {
        // For EF Core
    }

    public SheetId SheetId { get; private set; }
    public CellValue CellValue { get; private set; }
    public Sheet Sheet { get; }

    public static Result<Cell> Create(CellId id, string expression, SheetId sheetId, Sheet sheet)
    {
        var creationResult = CellValue.Create(expression, sheet);

        return creationResult.IsFailure
            ? Result.Failure<Cell>(Error.DefaultError)
            : new Cell(id, creationResult.Value, sheetId, sheet);
    }

    public Result Update(string expression)
    {
        var creationResult = CellValue.Create(expression, Sheet);

        if (creationResult.IsFailure) return Result.Failure<Cell>(creationResult.Error);

        CellValue = creationResult.Value;

        return Result.Success(this);
    }
}