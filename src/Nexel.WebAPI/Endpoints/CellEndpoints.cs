using MediatR;
using Nexel.Application.Features.Cells.Queries;

namespace Nexel.WebAPI.Endpoints;

public static class CellEndpoints
{
    public static void MapCellEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1");

        group.MapGet("{sheetId}/{cellId}", GetCellById);
    }

    private static async Task<IResult> GetCellById(
        string sheetId,
        string cellId,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var getCellbyIdQuery = new GetCellByIdQuery(sheetId, cellId);

        var result = await sender.Send(getCellbyIdQuery, cancellationToken);

        return result.IsSuccess
            ? Results.Json(result.Value, statusCode: 200)
            : Results.Json(result.Error, statusCode: 404);
    }
}