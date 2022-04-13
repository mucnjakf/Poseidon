#nullable disable

namespace Poseidon.DAL.Migrations;

public partial class AddedIsDismissedToEvent : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "IsDismissed",
            table: "Events",
            type: "bit",
            nullable: false,
            defaultValue: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsDismissed",
            table: "Events");
    }
}