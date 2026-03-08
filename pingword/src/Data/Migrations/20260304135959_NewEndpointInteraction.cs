using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pingword.src.Data.Migrations
{
    /// <inheritdoc />
    public partial class NewEndpointInteraction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InteractionEnum",
                table: "Words",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InteractionEnum",
                table: "Words");
        }
    }
}
