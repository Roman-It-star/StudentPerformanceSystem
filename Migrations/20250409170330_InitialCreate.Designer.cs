﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudentPerformanceSystem.Models;

#nullable disable

namespace StudentPerformanceSystem.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250409170330_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("StudentPerformanceSystem.Models.Score", b =>
                {
                    b.Property<int>("ScoreId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ScoreId"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.Property<int>("SubjectId")
                        .HasColumnType("int");

                    b.HasKey("ScoreId");

                    b.HasIndex("StudentId");

                    b.HasIndex("SubjectId");

                    b.ToTable("Scores");
                });

            modelBuilder.Entity("StudentPerformanceSystem.Models.Student", b =>
                {
                    b.Property<int>("StudentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StudentId"));

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Group")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StudentId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("StudentPerformanceSystem.Models.Subject", b =>
                {
                    b.Property<int>("SubjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SubjectId"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SubjectId");

                    b.ToTable("Subjects");

                    b.HasData(
                        new
                        {
                            SubjectId = 1,
                            Name = "Математика"
                        },
                        new
                        {
                            SubjectId = 2,
                            Name = "Физика"
                        },
                        new
                        {
                            SubjectId = 3,
                            Name = "Программирование"
                        });
                });

            modelBuilder.Entity("StudentPerformanceSystem.Models.Score", b =>
                {
                    b.HasOne("StudentPerformanceSystem.Models.Student", "Student")
                        .WithMany("Scores")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudentPerformanceSystem.Models.Subject", "Subject")
                        .WithMany("Scores")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("StudentPerformanceSystem.Models.Student", b =>
                {
                    b.Navigation("Scores");
                });

            modelBuilder.Entity("StudentPerformanceSystem.Models.Subject", b =>
                {
                    b.Navigation("Scores");
                });
#pragma warning restore 612, 618
        }
    }
}
