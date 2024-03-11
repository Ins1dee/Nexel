using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexel.Domain.Modules.Cells;
using Nexel.Domain.Modules.Cells.ValueObjects;

namespace Nexel.Persistence.Configurations;

public sealed class CellConfiguration : IEntityTypeConfiguration<Cell>
{
    public void Configure(EntityTypeBuilder<Cell> builder)
    {
        builder.HasKey(cell => cell.Id);
        builder.Property(cell => cell.Id)
            .HasConversion(
                cellId => cellId.Value,
                value => new CellId(value));

        builder.OwnsOne(cell => cell.CellValue, cellValue =>
        {
            cellValue.Property(cellValue => cellValue.Value)
                .HasColumnName("Value");

            cellValue.Property(cellValue => cellValue.ResultValue)
                .HasColumnName("Result");
        });

        builder.HasOne(cell => cell.Sheet)
            .WithMany(sheet => sheet.Cells)
            .HasForeignKey(cell => cell.SheetId);
    }
}