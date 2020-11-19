using Microsoft.EntityFrameworkCore.Migrations;

namespace FluentBlog.Migrations
{
    public partial class addarchivescount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Name", "Value" },
                values: new object[] { "ArchivesCountPerPage", "5" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Name",
                keyValue: "ArchivesCountPerPage");
        }
    }
}
