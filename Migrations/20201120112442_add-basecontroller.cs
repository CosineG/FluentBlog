using Microsoft.EntityFrameworkCore.Migrations;

namespace FluentBlog.Migrations
{
    public partial class addbasecontroller : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Name", "Value" },
                values: new object[] { "BloggerName", "余弦G" });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Name", "Value" },
                values: new object[] { "BlogIntro", "现在向第一缕阳光宣誓，走出尘埃与那茫然彷徨。" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Name",
                keyValue: "BloggerName");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Name",
                keyValue: "BlogIntro");
        }
    }
}
