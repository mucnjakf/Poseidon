#nullable disable

namespace Poseidon.DAL.Migrations;

public partial class AddedMlScoreAndProbabilityToEvent : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<float>(
            name: "PredictionProbability",
            table: "Events",
            type: "real",
            nullable: false,
            defaultValue: 100f);

        migrationBuilder.AddColumn<float>(
            name: "PredictionScore",
            table: "Events",
            type: "real",
            nullable: false,
            defaultValue: 1f);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "PredictionProbability",
            table: "Events");

        migrationBuilder.DropColumn(
            name: "PredictionScore",
            table: "Events");
    }
}