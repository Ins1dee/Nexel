using Microsoft.EntityFrameworkCore;
using Nexel.Application.Abstractions;
using Nexel.Domain.Modules.Cells;
using Nexel.Domain.Modules.Sheets;

namespace Nexel.Persistence;

public class ApplicationDbContext : DbContext, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public required DbSet<Sheet> Sheets { get; set; }
    public required DbSet<Cell> Cells { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}