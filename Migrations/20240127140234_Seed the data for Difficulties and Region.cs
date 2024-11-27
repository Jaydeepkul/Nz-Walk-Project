using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NZWalks.Migrations
{
    /// <inheritdoc />
    public partial class SeedthedataforDifficultiesandRegion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("1a438f00-9979-4ed9-8939-ef3c9743173b"), "Medium" },
                    { new Guid("5816bfcf-d130-49ef-8b48-bc574a93f257"), "Easy" },
                    { new Guid("e0da852d-4ef3-46cb-b4d1-74219f74cd70"), "Hard" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("1a438f00-9979-4ed9-8939-ef3c9743173b"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("5816bfcf-d130-49ef-8b48-bc574a93f257"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("e0da852d-4ef3-46cb-b4d1-74219f74cd70"));
        }
    }
}
