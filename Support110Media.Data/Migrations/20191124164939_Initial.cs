using Microsoft.EntityFrameworkCore.Migrations;

namespace Support110Media.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CostumerModel",
                columns: table => new
                {
                    CostumerId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    CostumerName = table.Column<string>(nullable: true),
                    CostumerSurname = table.Column<string>(nullable: true),
                    CostumerPhoneNumber = table.Column<string>(nullable: true),
                    CostumerAddreess = table.Column<string>(nullable: true),
                    CostumerType = table.Column<string>(nullable: true),
                    CostumerMailAddress = table.Column<string>(nullable: true),
                    CostumerPassword = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostumerModel", x => x.CostumerId);
                });

            migrationBuilder.CreateTable(
                name: "UserModel",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    UserName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    MailAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserModel", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "FileModel",
                columns: table => new
                {
                    FileId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    FileName = table.Column<string>(nullable: true),
                    FileUploadDate = table.Column<string>(nullable: true),
                    CallDate = table.Column<string>(nullable: true),
                    CallTime = table.Column<string>(nullable: true),
                    CostumerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileModel", x => x.FileId);
                    table.ForeignKey(
                        name: "FK_FileModel_CostumerModel_CostumerId",
                        column: x => x.CostumerId,
                        principalTable: "CostumerModel",
                        principalColumn: "CostumerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileModel_CostumerId",
                table: "FileModel",
                column: "CostumerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileModel");

            migrationBuilder.DropTable(
                name: "UserModel");

            migrationBuilder.DropTable(
                name: "CostumerModel");
        }
    }
}
