using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pingword.src.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDaysInterected : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DaysInteractedCount",
                table: "Studies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaysInteractedCount",
                table: "Studies");
        }
    }
}
