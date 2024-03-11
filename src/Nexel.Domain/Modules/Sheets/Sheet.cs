using Nexel.Domain.Abstractions;
using Nexel.Domain.Modules.Cells;
using Nexel.Domain.Modules.Cells.ValueObjects;
using Nexel.Domain.Modules.Sheets.ValueObjects;
using Nexel.Domain.Shared;

namespace Nexel.Domain.Modules.Sheets;

public class Sheet : AggragateRoot<SheetId>
{
    private readonly List<Cell> _cells = new();

    public Sheet(SheetId id) : base(id)
    {
    }

    private Sheet()
    {
        // For EF Core
    }

    public IReadOnlyList<Cell> Cells => _cells;

    public Result<double> UpsertCell(CellId cellId, string expression)
    {
        var cell = _cells
            .SingleOrDefault(c => c.Id == cellId);

        if (cell is null)
        {
            var createResult = Cell.Create(
                cellId,
                expression,
                Id,
                this);

            if (createResult.IsFailure) return Result.Failure<double>(createResult.Error);

            _cells.Add(createResult.Value);

            return createResult.Value.CellValue.ResultValue;
        }

        var updateResult = cell.Update(expression);

        return updateResult.IsFailure
            ? Result.Failure<double>(updateResult.Error)
            : cell.CellValue.ResultValue;
    }
}