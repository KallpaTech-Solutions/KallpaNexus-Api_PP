using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KallpaNexus_API.Migrations
{
    /// <inheritdoc />
    public partial class AddAnonymousRecommendations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tipo",
                table: "AnalyticsEvents",
                newName: "EventType");

            migrationBuilder.RenameColumn(
                name: "Elemento",
                table: "AnalyticsEvents",
                newName: "TargetName");

            migrationBuilder.RenameColumn(
                name: "Fecha",
                table: "AnalyticsEvents",
                newName: "Timestamp");

            migrationBuilder.AddColumn<string>(
                name: "DeviceType",
                table: "AnalyticsEvents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "AnalyticsEvents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Referrer",
                table: "AnalyticsEvents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "AnalyticsEvents",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AnonymousRecommendations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Message = table.Column<string>(type: "text", nullable: false),
                    PagePath = table.Column<string>(type: "text", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnonymousRecommendations", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnonymousRecommendations");

            migrationBuilder.DropColumn(
                name: "DeviceType",
                table: "AnalyticsEvents");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "AnalyticsEvents");

            migrationBuilder.DropColumn(
                name: "Referrer",
                table: "AnalyticsEvents");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "AnalyticsEvents");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "AnalyticsEvents",
                newName: "Fecha");

            migrationBuilder.RenameColumn(
                name: "TargetName",
                table: "AnalyticsEvents",
                newName: "Elemento");

            migrationBuilder.RenameColumn(
                name: "EventType",
                table: "AnalyticsEvents",
                newName: "Tipo");
        }
    }
}
