using Nexel.Application.Abstractions.Messaging;

namespace Nexel.Application.Features.Cells.Queries;

public sealed record GetCellByIdQuery(string SheetId, string CellId) : IQuery<CellResponse>;