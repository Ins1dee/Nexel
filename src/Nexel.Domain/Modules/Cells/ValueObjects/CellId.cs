namespace Nexel.Domain.Modules.Cells.ValueObjects;

public record CellId
{
    public CellId(string value)
    {
        Value = value.ToLowerInvariant();
    }

    public string Value { get; init; }
}