using Microsoft.EntityFrameworkCore.Migrations;

namespace FluentBlog.Migrations
{
    public partial class add_donate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Name", "Value" },
                values: new object[] { "DonateSwitch", "on" });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Name", "Value" },
                values: new object[] { "DonateAlipay", "" });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Name", "Value" },
                values: new object[] { "DonateWechat", "" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Name",
                keyValue: "DonateAlipay");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Name",
                keyValue: "DonateSwitch");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Name",
                keyValue: "DonateWechat");
        }
    }
}
