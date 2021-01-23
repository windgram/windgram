using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Windgram.Identity.Infrastructure.Migrations
{
    public partial class ExternalLoginAddDateField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                schema: "identity",
                table: "user_login",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                schema: "identity",
                table: "user_login");
        }
    }
}
