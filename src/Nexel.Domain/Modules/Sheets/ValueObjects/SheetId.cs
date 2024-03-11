namespace Nexel.Domain.Modules.Sheets.ValueObjects;

public record SheetId
{
    public SheetId(string value)
    {
        Value = value.ToLowerInvariant();
    }

    public string Value { get; init; }
}