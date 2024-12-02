using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class MediCareAddManagerUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [Security].[Users] ([Id], [FirstName], [LastName], [ProfilPicture], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'16db7ed7-b345-4200-a218-90037b691199', N'Hiba', N'Rose', NULL, N'Hiba_Rose', N'HIBA_ROSE', N'hw.manager70tec@gmail.com', N'HW.MANAGER70TEC@GMAIL.COM', 0, N'AQAAAAIAAYagAAAAEB5UF4J/jCBNwS7KvFKCgtV4arnFqSx4ueMQjxI0s29F11aQURcqvVA0bUYVpYWfdA==', N'Z4B6PJQCH36ATFUC7HOZYZKRB663VUYK', N'78cc67c2-1c8f-4b9c-822c-b5fa9823956d', N'00201224619443', 0, 0, NULL, 1, 0)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [Security].[Users] WHERE Id = '16db7ed7-b345-4200-a218-90037b691199' ");
        }
    }
}
