using FluentValidation;
using Nexel.Application.Extensions;

namespace Nexel.Application.Features.Sheets.Commands.UpsertSheetWithCell;

internal class UpsertSheetWithCellCommandValidator : AbstractValidator<UpsertSheetWithCellCommand>
{
    public UpsertSheetWithCellCommandValidator()
    {
        RuleFor(x => x.CellId).IsCellOrSheet();

        RuleFor(x => x.SheetId).IsCellOrSheet();

        RuleFor(x => x.Expression).NotEmpty();
    }
}