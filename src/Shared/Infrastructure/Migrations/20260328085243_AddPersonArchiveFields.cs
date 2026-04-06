using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduCrm.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPersonArchiveFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "internal_notes",
                table: "people_persons");

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at_utc",
                table: "program_programs",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "program_programs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at_utc",
                table: "people_persons",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "people_persons",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "person_follow_ups",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    organization_id = table.Column<Guid>(type: "uuid", nullable: false),
                    person_id = table.Column<Guid>(type: "uuid", nullable: false),
                    program_id = table.Column<Guid>(type: "uuid", nullable: true),
                    type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    note = table.Column<string>(type: "text", nullable: true),
                    due_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    snoozed_until_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    completed_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    cancelled_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_person_follow_ups", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "person_follow_ups");

            migrationBuilder.DropColumn(
                name: "archived_at_utc",
                table: "program_programs");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "program_programs");

            migrationBuilder.DropColumn(
                name: "archived_at_utc",
                table: "people_persons");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "people_persons");

            migrationBuilder.AddColumn<string>(
                name: "internal_notes",
                table: "people_persons",
                type: "text",
                nullable: true);
        }
    }
}
