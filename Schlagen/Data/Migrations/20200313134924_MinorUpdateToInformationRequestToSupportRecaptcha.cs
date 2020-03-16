using Microsoft.EntityFrameworkCore.Migrations;

namespace Schlagen.Data.Migrations
{
    public partial class MinorUpdateToInformationRequestToSupportRecaptcha : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Details",
                schema: "aic",
                table: "InformationRequests",
                maxLength: 400,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReCaptcha",
                schema: "aic",
                table: "InformationRequests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReCaptcha",
                schema: "aic",
                table: "InformationRequests");

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                schema: "aic",
                table: "InformationRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 400);
        }
    }
}
