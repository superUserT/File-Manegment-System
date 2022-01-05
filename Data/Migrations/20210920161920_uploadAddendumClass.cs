using Microsoft.EntityFrameworkCore.Migrations;

namespace FreshGoldPractice2.Data.Migrations
{
    public partial class uploadAddendumClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "addendumUploads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocCodeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PalletId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganisationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommodityId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VarietyId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pack = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Grade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SizeCount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FarmId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetMarket = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CartonQuant = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PalletQuant = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntakeDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginDepot = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InspectionDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrigIntakeDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Orchard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConsNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Weight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetRegion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PackhouseCodeId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addendumUploads", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "addendumUploads");
        }
    }
}
