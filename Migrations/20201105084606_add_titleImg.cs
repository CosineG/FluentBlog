using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FluentBlog.Migrations
{
    public partial class add_titleImg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Modified",
                table: "Archives");

            migrationBuilder.AddColumn<string>(
                name: "TitleImage",
                table: "Archives",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Archives",
                keyColumn: "Aid",
                keyValue: 1,
                columns: new[] { "Text", "Uid" },
                values: new object[] { "欢迎使用Fluent Blog。这里是使用了asp.net core构建的博客框架。", "d89af503-9855-465b-a575-fffa345f97e6" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TitleImage",
                table: "Archives");

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified",
                table: "Archives",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Archives",
                keyColumn: "Aid",
                keyValue: 1,
                columns: new[] { "Text", "Uid" },
                values: new object[] { "欢迎使用Fluent Blog", null });
        }
    }
}
