﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NutrionE.Models;

#nullable disable

namespace NutrionE.Migrations
{
    [DbContext(typeof(NutrioneContext))]
    [Migration("20240408154022_UpdateInitialDB")]
    partial class UpdateInitialDB
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("NutrionE.Models.DailyDiet", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("DailyDiets");
                });

            modelBuilder.Entity("NutrionE.Models.DailyRoutine", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("DailyRoutines");
                });

            modelBuilder.Entity("NutrionE.Models.Exercise", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int>("CaloriesBurned")
                        .HasColumnType("int");

                    b.Property<long>("DailyRoutineId")
                        .HasColumnType("bigint");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<int>("ExerciseType")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DailyRoutineId");

                    b.ToTable("Exercise");
                });

            modelBuilder.Entity("NutrionE.Models.Meal", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int>("Calories")
                        .HasColumnType("int");

                    b.Property<int>("Carbs")
                        .HasColumnType("int");

                    b.Property<long>("DailyDietId")
                        .HasColumnType("bigint");

                    b.Property<string>("Dessert")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DietType")
                        .HasColumnType("int");

                    b.Property<int>("Fat")
                        .HasColumnType("int");

                    b.Property<string>("FirstPlate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Protein")
                        .HasColumnType("int");

                    b.Property<string>("SecondPlate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DailyDietId");

                    b.ToTable("Meals");
                });

            modelBuilder.Entity("NutrionE.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("DietType")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("EnrollmentDate")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("NutrionE.Models.DailyDiet", b =>
                {
                    b.HasOne("NutrionE.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("NutrionE.Models.DailyRoutine", b =>
                {
                    b.HasOne("NutrionE.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("NutrionE.Models.Exercise", b =>
                {
                    b.HasOne("NutrionE.Models.DailyRoutine", "DailyRoutine")
                        .WithMany("Exercises")
                        .HasForeignKey("DailyRoutineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DailyRoutine");
                });

            modelBuilder.Entity("NutrionE.Models.Meal", b =>
                {
                    b.HasOne("NutrionE.Models.DailyDiet", "DailyDiet")
                        .WithMany("Meals")
                        .HasForeignKey("DailyDietId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DailyDiet");
                });

            modelBuilder.Entity("NutrionE.Models.DailyDiet", b =>
                {
                    b.Navigation("Meals");
                });

            modelBuilder.Entity("NutrionE.Models.DailyRoutine", b =>
                {
                    b.Navigation("Exercises");
                });
#pragma warning restore 612, 618
        }
    }
}
