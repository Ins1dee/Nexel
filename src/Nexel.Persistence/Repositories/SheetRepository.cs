using Microsoft.EntityFrameworkCore;
using Nexel.Domain.Modules.Sheets;
using Nexel.Domain.Modules.Sheets.ValueObjects;

namespace Nexel.Persistence.Repositories;

public class SheetRepository : ISheetRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SheetRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Sheet?> GetByIdAsync(SheetId id)
    {
        return await _dbContext.Set<Sheet>()
            .Include(x => x.Cells)
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(Sheet sheet)
    {
        await _dbContext.AddAsync(sheet);
    }

    public void Update(Sheet sheet)
    {
        _dbContext.Update(sheet);
    }
}