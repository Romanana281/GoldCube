using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GoldCube.Migrations
{
    /// <inheritdoc />
    public partial class goldcube : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    ConvId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "text", nullable: true),
                    ResultValue = table.Column<int>(type: "integer", nullable: true),
                    SecretWord = table.Column<string>(type: "text", nullable: true),
                    Hash = table.Column<string>(type: "text", nullable: true),
                    Time = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.ConvId);
                    table.ForeignKey(
                        name: "FK_Games_Cell_ResultValue",
                        column: x => x.ResultValue,
                        principalTable: "Cell",
                        principalColumn: "Value");
                });

            migrationBuilder.CreateTable(
                name: "TemplateBank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    GameConvId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateBank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateBank_Games_GameConvId",
                        column: x => x.GameConvId,
                        principalTable: "Games",
                        principalColumn: "ConvId");
                    table.ForeignKey(
                        name: "FK_TemplateBank_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_ResultValue",
                table: "Games",
                column: "ResultValue");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateBank_GameConvId",
                table: "TemplateBank",
                column: "GameConvId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateBank_UserId",
                table: "TemplateBank",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TemplateBank");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Cell");
        }
    }
}
