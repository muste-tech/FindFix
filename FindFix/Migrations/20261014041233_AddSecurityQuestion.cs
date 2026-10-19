using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FindFix.Migrations
{
    /// <inheritdoc />
    public partial class AddSecurityQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_JobRequestId_Unique",
                table: "Reviews",
                column: "JobRequestId",
                unique: true);

                 migrationBuilder.DropIndex(
                name: "IX_Reviews_JobRequestId",
                table: "Reviews");

                 migrationBuilder.RenameIndex(
                name: "IX_Reviews_JobRequestId_Unique",
                table: "Reviews",
                newName: "IX_Reviews_JobRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reviews_JobRequestId",
                table: "Reviews");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_JobRequestId",
                table: "Reviews",
                column: "JobRequestId");
        }
    }
}
