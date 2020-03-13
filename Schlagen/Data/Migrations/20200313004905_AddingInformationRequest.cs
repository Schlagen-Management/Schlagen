using Microsoft.EntityFrameworkCore.Migrations;

namespace Schlagen.Data.Migrations
{
    public partial class AddingInformationRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "aic");

            migrationBuilder.CreateTable(
                name: "InformationTypes",
                schema: "aic",
                columns: table => new
                {
                    InformationTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InformationTypes", x => x.InformationTypeId);
                });

            migrationBuilder.CreateTable(
                name: "InformationRequests",
                schema: "aic",
                columns: table => new
                {
                    InformationRegardingId = table.Column<int>(nullable: false),
                    InformationRequestId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: false),
                    Phone = table.Column<string>(maxLength: 25, nullable: false),
                    Details = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InformationRequests", x => x.InformationRegardingId);
                    table.ForeignKey(
                        name: "FK_InformationRequests_InformationTypes_InformationRegardingId",
                        column: x => x.InformationRegardingId,
                        principalSchema: "aic",
                        principalTable: "InformationTypes",
                        principalColumn: "InformationTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "5thFloor",
                table: "EmploymentTypes",
                keyColumn: "EmploymentTypeId",
                keyValue: 1,
                column: "Name",
                value: "Consultant");

            migrationBuilder.UpdateData(
                schema: "5thFloor",
                table: "EmploymentTypes",
                keyColumn: "EmploymentTypeId",
                keyValue: 2,
                column: "Name",
                value: "Employee");

            migrationBuilder.InsertData(
                schema: "aic",
                table: "InformationTypes",
                columns: new[] { "InformationTypeId", "Name" },
                values: new object[,]
                {
                    { 1, "Dedicated Office" },
                    { 2, "Flexible Desk" },
                    { 3, "Virtual Office" },
                    { 4, "Conference Room" },
                    { 5, "Training Room" },
                    { 6, "CoPlay Space" },
                    { 7, "Craft Center" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InformationRequests",
                schema: "aic");

            migrationBuilder.DropTable(
                name: "InformationTypes",
                schema: "aic");

            migrationBuilder.UpdateData(
                schema: "5thFloor",
                table: "EmploymentTypes",
                keyColumn: "EmploymentTypeId",
                keyValue: 1,
                column: "Name",
                value: "1099");

            migrationBuilder.UpdateData(
                schema: "5thFloor",
                table: "EmploymentTypes",
                keyColumn: "EmploymentTypeId",
                keyValue: 2,
                column: "Name",
                value: "W2");
        }
    }
}
