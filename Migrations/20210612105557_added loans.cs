using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace vfinance_api.Migrations
{
    public partial class addedloans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoanId",
                table: "Attachments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LoanNumber = table.Column<string>(type: "varchar(95)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    LoanDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LoanTerm = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PrincipalAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    InterestRate = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    TotalInterest = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    TotalPayment = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    PaymentStartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PaymentAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    TotalPaidAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    BalanceAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReferredBYId = table.Column<int>(type: "int", nullable: true),
                    CreationAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanId", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loans_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Loans_Customers_ReferredBYId",
                        column: x => x.ReferredBYId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_LoanId",
                table: "Attachments",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "Idx_LoanNumber",
                table: "Loans",
                column: "LoanNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Loans_CustomerId",
                table: "Loans",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_ReferredBYId",
                table: "Loans",
                column: "ReferredBYId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Loans_LoanId",
                table: "Attachments",
                column: "LoanId",
                principalTable: "Loans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Loans_LoanId",
                table: "Attachments");

            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_LoanId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "LoanId",
                table: "Attachments");
        }
    }
}
