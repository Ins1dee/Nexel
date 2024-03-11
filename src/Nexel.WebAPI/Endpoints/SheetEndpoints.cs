using MediatR;
using Nexel.Application.Features.Sheets.Commands.UpsertSheetWithCell;
using Nexel.Application.Features.Sheets.Queries;
using Nexel.WebAPI.Requests;

namespace Nexel.WebAPI.Endpoints;

public static class SheetEndpoints
{
    public static void MapSheetEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1");

        group.MapPost("{sheetId}/{cellId}", UpsertSheetAndCell);
        group.MapGet("{sheetId}", GetSheetById);
    }

    private static async Task<IResult> UpsertSheetAndCell(
        string sheetId,
        string cellId,
        UpsertSheetAndCellRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var upsertSheetWithCellCommand = new UpsertSheetWithCellCommand(
            sheetId,
            cellId,
            request.Value);

        var result = await sender.Send(upsertSheetWithCellCommand, cancellationToken);

        return result.IsSuccess
            ? Results.Json(result.Value, statusCode: 201)
            : Results.Json(result.Value, statusCode: 422);
    }

    private static async Task<IResult> GetSheetById(
        string sheetId,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var getSheetbyIdQuery = new GetSheetByIdQuery(sheetId);

        var result = await sender.Send(getSheetbyIdQuery, cancellationToken);

        return result.IsSuccess
            ? Results.Json(result.Value, statusCode: 200)
            : Results.Json(result.Error, statusCode: 404);
    }
}