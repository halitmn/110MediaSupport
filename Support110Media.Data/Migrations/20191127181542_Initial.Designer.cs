﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Support110Media.Data.Context;

namespace Support110Media.Data.Migrations
{
    [DbContext(typeof(MasterContext))]
    [Migration("20191127181542_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099");

            modelBuilder.Entity("Support110Media.Data.Model.CostumerModel", b =>
                {
                    b.Property<int>("CostumerId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompanyName")
                        .IsRequired();

                    b.Property<string>("CostumerAddreess");

                    b.Property<string>("CostumerMailAddress");

                    b.Property<string>("CostumerName")
                        .IsRequired();

                    b.Property<string>("CostumerPassword")
                        .IsRequired();

                    b.Property<string>("CostumerPhoneNumber");

                    b.Property<string>("CostumerSurname")
                        .IsRequired();

                    b.Property<string>("CostumerType");

                    b.HasKey("CostumerId");

                    b.ToTable("CostumerModel");
                });

            modelBuilder.Entity("Support110Media.Data.Model.FileModel", b =>
                {
                    b.Property<int>("FileId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CallDate")
                        .IsRequired();

                    b.Property<string>("CallTime")
                        .IsRequired();

                    b.Property<int>("CostumerId");

                    b.Property<string>("FileName")
                        .IsRequired();

                    b.Property<string>("FilePath")
                        .IsRequired();

                    b.Property<string>("FileUploadDate")
                        .IsRequired();

                    b.HasKey("FileId");

                    b.HasIndex("CostumerId");

                    b.ToTable("FileModel");
                });

            modelBuilder.Entity("Support110Media.Data.Model.UserModel", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("MailAddress");

                    b.Property<string>("Password");

                    b.Property<string>("UserName");

                    b.HasKey("UserId");

                    b.ToTable("UserModel");
                });

            modelBuilder.Entity("Support110Media.Data.Model.FileModel", b =>
                {
                    b.HasOne("Support110Media.Data.Model.CostumerModel", "CostumerModel")
                        .WithMany("FileModel")
                        .HasForeignKey("CostumerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}