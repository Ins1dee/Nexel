using Nexel.Application.Abstractions.Messaging;
using Nexel.Application.Features.Cells;

namespace Nexel.Application.Features.Sheets.Queries;

public sealed record GetSheetByIdQuery(string SheetId) : IQuery<Dictionary<string, CellResponse>>;