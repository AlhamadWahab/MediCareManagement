using MediCareSecurity_IdentityManagementLayer;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class MediCareSeedRolesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData("Roles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] { Guid.NewGuid().ToString(), UserRole.User_Role, "User".ToUpper(), Guid.NewGuid().ToString() },
                schema: "Security"
            );

            migrationBuilder.InsertData("Roles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] { Guid.NewGuid().ToString(), UserRole.ManagerRole, "Manager".ToUpper(), Guid.NewGuid().ToString() },
                schema: "Security"
            );

            migrationBuilder.InsertData("Roles",
               columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
               values: new object[] { Guid.NewGuid().ToString(), UserRole.DoctorRole, "Doctor".ToUpper(), Guid.NewGuid().ToString() },
               schema: "Security"
            );

            migrationBuilder.InsertData("Roles",
               columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
               values: new object[] { Guid.NewGuid().ToString(), UserRole.PatientRole, "Patient".ToUpper(), Guid.NewGuid().ToString() },
               schema: "Security"
            );

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete FROM[Security].[Roles]");
        }

    }
}
