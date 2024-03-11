using Nexel.Domain.Modules.Sheets.ValueObjects;

namespace Nexel.Domain.Modules.Sheets;

public interface ISheetRepository
{
    Task AddAsync(Sheet sheet);
    Task<Sheet?> GetByIdAsync(SheetId id);
    public void Update(Sheet sheet);
}