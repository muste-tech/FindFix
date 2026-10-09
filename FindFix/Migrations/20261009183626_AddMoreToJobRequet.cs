using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FindFix.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreToJobRequet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "JobRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_JobRequests_ServiceId",
                table: "JobRequests",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobRequests_Services_ServiceId",
                table: "JobRequests",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobRequests_Services_ServiceId",
                table: "JobRequests");

            migrationBuilder.DropIndex(
                name: "IX_JobRequests_ServiceId",
                table: "JobRequests");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "JobRequests");
        }
    }
}
