using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lesson2.Migrations
{
    /// <inheritdoc />
    public partial class PersistSenderTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "Senders");

            migrationBuilder.CreateTable(
                name: "SenderTokens",
                columns: table => new
                {
                    TokenHash = table.Column<string>(type: "TEXT", nullable: false),
                    SenderEmail = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SenderTokens", x => x.TokenHash);
                    table.ForeignKey(
                        name: "FK_SenderTokens_Senders_SenderEmail",
                        column: x => x.SenderEmail,
                        principalTable: "Senders",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SenderTokens_SenderEmail",
                table: "SenderTokens",
                column: "SenderEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SenderTokens");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Senders",
                type: "TEXT",
                nullable: true);
        }
    }
}
