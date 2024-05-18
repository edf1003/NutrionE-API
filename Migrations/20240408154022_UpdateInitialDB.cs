using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NutrionE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInitialDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meals_DailyDates_DailyDateId",
                table: "Meals");

            migrationBuilder.DropTable(
                name: "DailyDates");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "DailyDateId",
                table: "Meals",
                newName: "DailyDietId");

            migrationBuilder.RenameIndex(
                name: "IX_Meals_DailyDateId",
                table: "Meals",
                newName: "IX_Meals_DailyDietId");

            migrationBuilder.AddColumn<int>(
                name: "DietType",
                table: "Meals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DailyDiets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyDiets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyDiets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyRoutines",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyRoutines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyRoutines_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Exercise",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    CaloriesBurned = table.Column<int>(type: "int", nullable: false),
                    ExerciseType = table.Column<int>(type: "int", nullable: false),
                    DailyRoutineId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exercise_DailyRoutines_DailyRoutineId",
                        column: x => x.DailyRoutineId,
                        principalTable: "DailyRoutines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyDiets_UserId",
                table: "DailyDiets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyRoutines_UserId",
                table: "DailyRoutines",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_DailyRoutineId",
                table: "Exercise",
                column: "DailyRoutineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meals_DailyDiets_DailyDietId",
                table: "Meals",
                column: "DailyDietId",
                principalTable: "DailyDiets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meals_DailyDiets_DailyDietId",
                table: "Meals");

            migrationBuilder.DropTable(
                name: "DailyDiets");

            migrationBuilder.DropTable(
                name: "Exercise");

            migrationBuilder.DropTable(
                name: "DailyRoutines");

            migrationBuilder.DropColumn(
                name: "DietType",
                table: "Meals");

            migrationBuilder.RenameColumn(
                name: "DailyDietId",
                table: "Meals",
                newName: "DailyDateId");

            migrationBuilder.RenameIndex(
                name: "IX_Meals_DailyDietId",
                table: "Meals",
                newName: "IX_Meals_DailyDateId");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "DailyDates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyDates_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyDates_UserId",
                table: "DailyDates",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meals_DailyDates_DailyDateId",
                table: "Meals",
                column: "DailyDateId",
                principalTable: "DailyDates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
