#nullable disable

namespace Poseidon.DAL.Migrations;

public partial class TweakedEventMlData : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "PredictionScore",
            table: "Events");

        migrationBuilder.RenameColumn(
            name: "PredictionProbability",
            table: "Events",
            newName: "IllegalEventProbability");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "IllegalEventProbability",
            table: "Events",
            newName: "PredictionProbability");

        migrationBuilder.AddColumn<float>(
            name: "PredictionScore",
            table: "Events",
            type: "real",
            nullable: false,
            defaultValue: 0f);
    }
}