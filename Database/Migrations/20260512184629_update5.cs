using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GoldCube.Migrations
{
    /// <inheritdoc />
    public partial class update5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Cell_ResultValue",
                table: "Games");

            migrationBuilder.DropTable(
                name: "Cell");

            migrationBuilder.DropIndex(
                name: "IX_Games_ResultValue",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "ResultValue",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "Result",
                table: "Games",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Result",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "ResultValue",
                table: "Games",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cell",
                columns: table => new
                {
                    Value = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Color = table.Column<int>(type: "integer", nullable: false),
                    IsEven = table.Column<bool>(type: "boolean", nullable: false),
                    Sector = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cell", x => x.Value);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_ResultValue",
                table: "Games",
                column: "ResultValue");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Cell_ResultValue",
                table: "Games",
                column: "ResultValue",
                principalTable: "Cell",
                principalColumn: "Value");
        }
    }
}
