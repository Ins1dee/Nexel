using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexel.Domain.Modules.Sheets;
using Nexel.Domain.Modules.Sheets.ValueObjects;

namespace Nexel.Persistence.Configurations;

public sealed class SheetConfiguration : IEntityTypeConfiguration<Sheet>
{
    public void Configure(EntityTypeBuilder<Sheet> builder)
    {
        builder.HasKey(sheet => sheet.Id);
        builder.Property(sheet => sheet.Id)
            .HasConversion(
                sheetId => sheetId.Value,
                value => new SheetId(value));

        builder.HasMany(sheet => sheet.Cells)
            .WithOne(cell => cell.Sheet)
            .HasForeignKey(cell => cell.SheetId);
    }
}