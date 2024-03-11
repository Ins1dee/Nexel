﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nexel.Persistence;

#nullable disable

namespace Nexel.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230930144133_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Nexel.Domain.Cells.Cell", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SheetId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("SheetId");

                    b.ToTable("Cells");
                });

            modelBuilder.Entity("Nexel.Domain.Sheets.Sheet", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.ToTable("Sheets");
                });

            modelBuilder.Entity("Nexel.Domain.Cells.Cell", b =>
                {
                    b.HasOne("Nexel.Domain.Sheets.Sheet", null)
                        .WithMany("Cells")
                        .HasForeignKey("SheetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Nexel.Domain.Cells.ValueObjects.CellValue", "CellValue", b1 =>
                        {
                            b1.Property<string>("CellId")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<double>("ResultValue")
                                .HasColumnType("float")
                                .HasColumnName("Result");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Value");

                            b1.HasKey("CellId");

                            b1.ToTable("Cells");

                            b1.WithOwner()
                                .HasForeignKey("CellId");
                        });

                    b.Navigation("CellValue")
                        .IsRequired();
                });

            modelBuilder.Entity("Nexel.Domain.Sheets.Sheet", b =>
                {
                    b.Navigation("Cells");
                });
#pragma warning restore 612, 618
        }
    }
}
