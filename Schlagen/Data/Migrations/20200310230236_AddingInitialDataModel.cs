using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Schlagen.Data.Migrations
{
    public partial class AddingInitialDataModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "5thFloor");

            migrationBuilder.CreateTable(
                name: "EmploymentLocations",
                schema: "5thFloor",
                columns: table => new
                {
                    EmploymentLocationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    StreetAddress = table.Column<string>(maxLength: 100, nullable: true),
                    City = table.Column<string>(maxLength: 100, nullable: true),
                    State = table.Column<string>(maxLength: 100, nullable: true),
                    PostalCode = table.Column<string>(maxLength: 25, nullable: true),
                    Latitude = table.Column<decimal>(type: "Decimal(9,6)", nullable: true),
                    Longitude = table.Column<decimal>(type: "Decimal(9,6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmploymentLocations", x => x.EmploymentLocationId);
                });

            migrationBuilder.CreateTable(
                name: "EmploymentTypes",
                schema: "5thFloor",
                columns: table => new
                {
                    EmploymentTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmploymentTypes", x => x.EmploymentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "JobTypes",
                schema: "5thFloor",
                columns: table => new
                {
                    JobTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTypes", x => x.JobTypeId);
                });

            migrationBuilder.CreateTable(
                name: "JobRequisitions",
                schema: "5thFloor",
                columns: table => new
                {
                    JobRequisitionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(maxLength: 1000, nullable: false),
                    Title = table.Column<string>(maxLength: 200, nullable: false),
                    DateToPost = table.Column<DateTime>(nullable: false),
                    DateToRemove = table.Column<DateTime>(nullable: true),
                    JobTypeId = table.Column<int>(nullable: false),
                    EmploymentLocationId = table.Column<int>(nullable: false),
                    EmploymentTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobRequisitions", x => x.JobRequisitionId);
                    table.ForeignKey(
                        name: "FK_JobRequisitions_EmploymentLocations_EmploymentLocationId",
                        column: x => x.EmploymentLocationId,
                        principalSchema: "5thFloor",
                        principalTable: "EmploymentLocations",
                        principalColumn: "EmploymentLocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobRequisitions_EmploymentTypes_EmploymentTypeId",
                        column: x => x.EmploymentTypeId,
                        principalSchema: "5thFloor",
                        principalTable: "EmploymentTypes",
                        principalColumn: "EmploymentTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobRequisitions_JobTypes_JobTypeId",
                        column: x => x.JobTypeId,
                        principalSchema: "5thFloor",
                        principalTable: "JobTypes",
                        principalColumn: "JobTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobApplicants",
                schema: "5thFloor",
                columns: table => new
                {
                    JobApplicantId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(maxLength: 100, nullable: false),
                    LastName = table.Column<string>(maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 50, nullable: false),
                    EmailAddress = table.Column<string>(maxLength: 100, nullable: false),
                    ResumeText = table.Column<string>(maxLength: 20000, nullable: false),
                    JobRequisitionId = table.Column<int>(nullable: false),
                    JobRequisitionId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobApplicants", x => x.JobApplicantId);
                    table.ForeignKey(
                        name: "FK_JobApplicants_JobRequisitions_JobRequisitionId",
                        column: x => x.JobRequisitionId,
                        principalSchema: "5thFloor",
                        principalTable: "JobRequisitions",
                        principalColumn: "JobRequisitionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobApplicants_JobRequisitions_JobRequisitionId1",
                        column: x => x.JobRequisitionId1,
                        principalSchema: "5thFloor",
                        principalTable: "JobRequisitions",
                        principalColumn: "JobRequisitionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "5thFloor",
                table: "EmploymentLocations",
                columns: new[] { "EmploymentLocationId", "City", "Latitude", "Longitude", "Name", "PostalCode", "State", "StreetAddress" },
                values: new object[] { 1, null, null, null, "Orlando", null, null, null });

            migrationBuilder.InsertData(
                schema: "5thFloor",
                table: "EmploymentTypes",
                columns: new[] { "EmploymentTypeId", "Name" },
                values: new object[,]
                {
                    { 1, "1099" },
                    { 2, "W2" }
                });

            migrationBuilder.InsertData(
                schema: "5thFloor",
                table: "JobTypes",
                columns: new[] { "JobTypeId", "Name" },
                values: new object[,]
                {
                    { 1, "Part Time" },
                    { 2, "Full Time" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobApplicants_JobRequisitionId",
                schema: "5thFloor",
                table: "JobApplicants",
                column: "JobRequisitionId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplicants_JobRequisitionId1",
                schema: "5thFloor",
                table: "JobApplicants",
                column: "JobRequisitionId1");

            migrationBuilder.CreateIndex(
                name: "IX_JobRequisitions_EmploymentLocationId",
                schema: "5thFloor",
                table: "JobRequisitions",
                column: "EmploymentLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_JobRequisitions_EmploymentTypeId",
                schema: "5thFloor",
                table: "JobRequisitions",
                column: "EmploymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_JobRequisitions_JobTypeId",
                schema: "5thFloor",
                table: "JobRequisitions",
                column: "JobTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobApplicants",
                schema: "5thFloor");

            migrationBuilder.DropTable(
                name: "JobRequisitions",
                schema: "5thFloor");

            migrationBuilder.DropTable(
                name: "EmploymentLocations",
                schema: "5thFloor");

            migrationBuilder.DropTable(
                name: "EmploymentTypes",
                schema: "5thFloor");

            migrationBuilder.DropTable(
                name: "JobTypes",
                schema: "5thFloor");
        }
    }
}
