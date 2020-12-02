using Microsoft.EntityFrameworkCore.Migrations;

namespace FluentBlog.Migrations
{
    public partial class addfeedswitch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Metas",
                keyColumn: "Mid",
                keyValue: 1,
                column: "Slug",
                value: "uncategorized");

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Name", "Value" },
                values: new object[] { "FeedSwitch", "on" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Name",
                keyValue: "FeedSwitch");

            migrationBuilder.UpdateData(
                table: "Metas",
                keyColumn: "Mid",
                keyValue: 1,
                column: "Slug",
                value: "Uncategorized");
        }
    }
}
