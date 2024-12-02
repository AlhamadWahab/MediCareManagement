using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class MediCareChangePraxisAdressAndTelefonFieldsToNotRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PraxisAdress",
                table: "Doctors",
                type: "nvarchar(100)", // Corrected from "navarchar(100)" to "nvarchar(100)"
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)", // Corrected type
                oldNullable: false
            );

            migrationBuilder.AlterColumn<string>(
               name: "Telefon",
               table: "Doctors",
               type: "nvarchar(100)",
               nullable: true,
               oldClrType: typeof(string),
               oldType: "nvarchar(100)", // Ensure this matches your schema
               oldNullable: false
           );

            migrationBuilder.AlterColumn<string>(
               name: "Specialty",
               table: "Doctors",
               type: "nvarchar(100)",
               nullable: true,
               oldClrType: typeof(string),
               oldType: "nvarchar(100)",
               oldNullable: false
           );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PraxisAdress",
                table: "Doctors",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true
            );

            migrationBuilder.AlterColumn<string>(
                name: "Specialty",
                table: "Doctors",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true
            );

            migrationBuilder.AlterColumn<string>(
                name: "Telefon",
                table: "Doctors",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true
            );
        }
    }
}
