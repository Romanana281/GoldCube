using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoldCube.Migrations
{
    /// <inheritdoc />
    public partial class update7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TemplateBank_Games_GameConvId",
                table: "TemplateBank");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateBank_Users_UserId",
                table: "TemplateBank");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TemplateBank",
                table: "TemplateBank");

            migrationBuilder.DropIndex(
                name: "IX_TemplateBank_GameConvId",
                table: "TemplateBank");

            migrationBuilder.DropColumn(
                name: "GameConvId",
                table: "TemplateBank");

            migrationBuilder.RenameTable(
                name: "TemplateBank",
                newName: "TemplateBanks");

            migrationBuilder.RenameIndex(
                name: "IX_TemplateBank_UserId",
                table: "TemplateBanks",
                newName: "IX_TemplateBanks_UserId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndAt",
                table: "Games",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "TemplateBanks",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "GameId",
                table: "TemplateBanks",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TemplateBanks",
                table: "TemplateBanks",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateBanks_GameId",
                table: "TemplateBanks",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateBanks_Games_GameId",
                table: "TemplateBanks",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "ConvId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateBanks_Users_UserId",
                table: "TemplateBanks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TemplateBanks_Games_GameId",
                table: "TemplateBanks");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateBanks_Users_UserId",
                table: "TemplateBanks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TemplateBanks",
                table: "TemplateBanks");

            migrationBuilder.DropIndex(
                name: "IX_TemplateBanks_GameId",
                table: "TemplateBanks");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "TemplateBanks");

            migrationBuilder.RenameTable(
                name: "TemplateBanks",
                newName: "TemplateBank");

            migrationBuilder.RenameIndex(
                name: "IX_TemplateBanks_UserId",
                table: "TemplateBank",
                newName: "IX_TemplateBank_UserId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndAt",
                table: "Games",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "TemplateBank",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "GameConvId",
                table: "TemplateBank",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TemplateBank",
                table: "TemplateBank",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateBank_GameConvId",
                table: "TemplateBank",
                column: "GameConvId");

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateBank_Games_GameConvId",
                table: "TemplateBank",
                column: "GameConvId",
                principalTable: "Games",
                principalColumn: "ConvId");

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateBank_Users_UserId",
                table: "TemplateBank",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
