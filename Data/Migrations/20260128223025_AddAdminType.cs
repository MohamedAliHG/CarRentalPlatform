using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgenceLocationVoiture.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateDerniereConnexion",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Departement",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateDerniereConnexion",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Departement",
                table: "AspNetUsers");
        }
    }
}
