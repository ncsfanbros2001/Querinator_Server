using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations.OperatorDb
{
    /// <inheritdoc />
    public partial class AddUserIdForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "SavedQuery",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_SavedQuery_UserId",
                table: "SavedQuery",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SavedQuery_AspNetUsers_UserId",
                table: "SavedQuery",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SavedQuery_AspNetUsers_UserId",
                table: "SavedQuery");

            migrationBuilder.DropIndex(
                name: "IX_SavedQuery_UserId",
                table: "SavedQuery");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SavedQuery");
        }
    }
}
