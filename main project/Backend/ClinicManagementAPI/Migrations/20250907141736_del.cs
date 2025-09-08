using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class del : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestRecord",
                table: "Appointments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TestRecord",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
