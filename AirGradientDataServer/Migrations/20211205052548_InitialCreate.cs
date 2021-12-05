using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirGradientDataServer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Measurements",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MeasurementTime = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')"),
                    DeviceId = table.Column<string>(type: "TEXT", nullable: true),
                    WifiStrength = table.Column<int>(type: "INTEGER", nullable: true),
                    PM25 = table.Column<int>(type: "INTEGER", nullable: true),
                    CO2 = table.Column<int>(type: "INTEGER", nullable: true),
                    Temperature = table.Column<float>(type: "REAL", nullable: true),
                    Humidity = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurements", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Measurements");
        }
    }
}
