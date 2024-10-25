﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ResourceServer.DataProtection;

#nullable disable

namespace ResourceServer.Migrations
{
    [DbContext(typeof(DataProtectionDbContext))]
    [Migration("20240917133009_dbdataprotection")]
    partial class dbdataprotection
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.2");

            modelBuilder.Entity("ResourceServer.DataProtection.DataProtectionElement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Xml")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("DataProtectionXMLElements");
                });
#pragma warning restore 612, 618
        }
    }
}