using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class MediCareAllowSpaceWithAlphabeticCharacter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "Appointments",
                nullable: true, 
                maxLength: 200, 
                oldClrType: typeof(string),
                oldType: "nvarchar(200)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
               name: "Reason",
               table: "Appointments",
               type: "nvarchar(200)",
               nullable: true,
               oldClrType: typeof(string),
               oldMaxLength: 200);
        }
    }
}
