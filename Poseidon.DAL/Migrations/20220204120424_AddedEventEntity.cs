#nullable disable

namespace Poseidon.DAL.Migrations;

public partial class AddedEventEntity : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Events",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Mmsi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Latitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Longitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                StartTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                EndTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                MedianSpeedKnots = table.Column<double>(type: "float", nullable: false),
                TotalDurationHours = table.Column<double>(type: "float", nullable: false),
                IsIllegal = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Events", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Events");
    }
}