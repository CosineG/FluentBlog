using Microsoft.EntityFrameworkCore.Migrations;

namespace FluentBlog.Migrations
{
    public partial class updatemetas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Metas",
                keyColumn: "Mid",
                keyValue: -1);

            migrationBuilder.DropColumn(
                name: "Count",
                table: "Metas");

            migrationBuilder.AddColumn<bool>(
                name: "Default",
                table: "Metas",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Metas",
                columns: new[] { "Mid", "Default", "Description", "Name", "Slug", "Type" },
                values: new object[] { 1, true, null, "未归类", "Uncategorized", "category" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Metas",
                keyColumn: "Mid",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "Default",
                table: "Metas");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Metas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Metas",
                columns: new[] { "Mid", "Count", "Description", "Name", "Slug", "Type" },
                values: new object[] { -1, 0, null, "未归类", "Uncategorized", "category" });
        }
    }
}
