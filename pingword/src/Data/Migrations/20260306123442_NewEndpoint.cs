using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pingword.src.Data.Migrations
{
    /// <inheritdoc />
    public partial class NewEndpoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeletd",
                table: "Words",
                newName: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Words",
                newName: "IsDeletd");
        }
    }
}
