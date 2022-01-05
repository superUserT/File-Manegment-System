using Microsoft.EntityFrameworkCore.Migrations;

namespace FreshGoldPractice2.Data.Migrations
{
    public partial class newMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UploadOnDatabaseId",
                table: "addendumUploads",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UploadOnSystemId",
                table: "addendumUploads",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_addendumUploads_UploadOnDatabaseId",
                table: "addendumUploads",
                column: "UploadOnDatabaseId");

            migrationBuilder.CreateIndex(
                name: "IX_addendumUploads_UploadOnSystemId",
                table: "addendumUploads",
                column: "UploadOnSystemId");

            migrationBuilder.AddForeignKey(
                name: "FK_addendumUploads_UploadOnDatabase_UploadOnDatabaseId",
                table: "addendumUploads",
                column: "UploadOnDatabaseId",
                principalTable: "UploadOnDatabase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_addendumUploads_UploadOnSystem_UploadOnSystemId",
                table: "addendumUploads",
                column: "UploadOnSystemId",
                principalTable: "UploadOnSystem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_addendumUploads_UploadOnDatabase_UploadOnDatabaseId",
                table: "addendumUploads");

            migrationBuilder.DropForeignKey(
                name: "FK_addendumUploads_UploadOnSystem_UploadOnSystemId",
                table: "addendumUploads");

            migrationBuilder.DropIndex(
                name: "IX_addendumUploads_UploadOnDatabaseId",
                table: "addendumUploads");

            migrationBuilder.DropIndex(
                name: "IX_addendumUploads_UploadOnSystemId",
                table: "addendumUploads");

            migrationBuilder.DropColumn(
                name: "UploadOnDatabaseId",
                table: "addendumUploads");

            migrationBuilder.DropColumn(
                name: "UploadOnSystemId",
                table: "addendumUploads");
        }
    }
}
