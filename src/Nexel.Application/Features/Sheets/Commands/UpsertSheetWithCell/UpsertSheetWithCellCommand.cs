using Nexel.Application.Abstractions.Messaging;
using Nexel.Application.Features.Cells;

namespace Nexel.Application.Features.Sheets.Commands.UpsertSheetWithCell;

public sealed record UpsertSheetWithCellCommand(
    string SheetId,
    string CellId,
    string Expression) : ICommand<CellResponse>;