﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PaletteMaster.Repository;

#nullable disable

namespace PaletteMaster.Repository.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.6");

            modelBuilder.Entity("PaletteMaster.Models.Domain.Color", b =>
                {
                    b.Property<int>("ColorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Hexadecimal")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("PaletteId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ColorId");

                    b.HasIndex("PaletteId");

                    b.ToTable("Colors");
                });

            modelBuilder.Entity("PaletteMaster.Models.Domain.Palette", b =>
                {
                    b.Property<int>("PaletteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("PaletteId");

                    b.ToTable("Palettes");
                });

            modelBuilder.Entity("PaletteMaster.Models.Domain.PaletteUseTracking", b =>
                {
                    b.Property<int>("PaletteUseTrackingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("PaletteId")
                        .HasColumnType("INTEGER");

                    b.HasKey("PaletteUseTrackingId");

                    b.HasIndex("PaletteId");

                    b.ToTable("PaletteUseTrackings");
                });

            modelBuilder.Entity("PaletteMaster.Models.Domain.Color", b =>
                {
                    b.HasOne("PaletteMaster.Models.Domain.Palette", null)
                        .WithMany("Colors")
                        .HasForeignKey("PaletteId");
                });

            modelBuilder.Entity("PaletteMaster.Models.Domain.PaletteUseTracking", b =>
                {
                    b.HasOne("PaletteMaster.Models.Domain.Palette", "Palette")
                        .WithMany("PaletteUseTrackings")
                        .HasForeignKey("PaletteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Palette");
                });

            modelBuilder.Entity("PaletteMaster.Models.Domain.Palette", b =>
                {
                    b.Navigation("Colors");

                    b.Navigation("PaletteUseTrackings");
                });
#pragma warning restore 612, 618
        }
    }
}
