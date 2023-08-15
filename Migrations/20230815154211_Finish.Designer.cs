﻿// <auto-generated />
using System;
using Backend;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Backend.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230815154211_Finish")]
    partial class Finish
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Backend.Models.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Ime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("KorisnickoIme")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Lozinka")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("LozinkaKljuc")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Prezime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tip")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Admini");
                });

            modelBuilder.Entity("Backend.Models.Glas", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("DatumGlasanja")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdIzbora")
                        .HasColumnType("int");

                    b.Property<int>("IdKorisnika")
                        .HasColumnType("int");

                    b.Property<int>("IdStranke")
                        .HasColumnType("int");

                    b.Property<string>("LokacijaGlasanja")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Glasovi");
                });

            modelBuilder.Entity("Backend.Models.Izbori", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("DatumPocetka")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DatumZavrsetka")
                        .HasColumnType("datetime2");

                    b.Property<string>("Grad")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Otvoreni")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Vrsta")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Izbori");
                });

            modelBuilder.Entity("Backend.Models.Korisnik", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("BrojTelefona")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DatumRodjenja")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Grad")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("KorisnickoIme")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Lozinka")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("LozinkaKljuc")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Pol")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Prezime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tip")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Korisnici");
                });

            modelBuilder.Entity("Backend.Models.Pripada", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("IdKorisnika")
                        .HasColumnType("int");

                    b.Property<int>("IdStranke")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Pripada");
                });

            modelBuilder.Entity("Backend.Models.Stranka", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("GodinaOsnivanja")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("KorisnickoIme")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Lozinka")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("LozinkaKljuc")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Naziv")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NosilacListe")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Slogan")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tip")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Stranke");
                });

            modelBuilder.Entity("Backend.Models.Ucestvujee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("IdIzbora")
                        .HasColumnType("int");

                    b.Property<int>("IdStranke")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Ucestvujee");
                });
#pragma warning restore 612, 618
        }
    }
}
