using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User_Management.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "bill",
                columns: table => new
                {
                    billId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    amount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.billId);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "BillTelcoservices",
                columns: table => new
                {
                    BillId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillTelcoservices", x => new { x.BillId, x.ServiceId });
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "telcoservices",
                columns: table => new
                {
                    serviceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    serviceName = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    monthlyCharge = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.serviceId);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "TelcoservicesUser",
                columns: table => new
                {
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<byte[]>(type: "binary(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelcoservicesUser", x => new { x.ServiceId, x.UserId });
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    userId = table.Column<byte[]>(type: "binary(16)", fixedLength: true, maxLength: 16, nullable: false),
                    name = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    userName = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phoneNumber = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.userId);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "servicebill",
                columns: table => new
                {
                    billId = table.Column<int>(type: "int", nullable: false),
                    serviceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.billId, x.serviceId })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "bill_bill_service",
                        column: x => x.billId,
                        principalTable: "bill",
                        principalColumn: "billId");
                    table.ForeignKey(
                        name: "service_bill_service",
                        column: x => x.serviceId,
                        principalTable: "telcoservices",
                        principalColumn: "serviceId");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "activatedservices",
                columns: table => new
                {
                    userId = table.Column<byte[]>(type: "binary(16)", fixedLength: true, maxLength: 16, nullable: false),
                    serviceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.userId, x.serviceId })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "fk_activatedservices_user",
                        column: x => x.userId,
                        principalTable: "user",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "service_active_user",
                        column: x => x.serviceId,
                        principalTable: "telcoservices",
                        principalColumn: "serviceId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "auth",
                columns: table => new
                {
                    userId = table.Column<byte[]>(type: "binary(16)", fixedLength: true, maxLength: 16, nullable: false),
                    password = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    salt = table.Column<byte[]>(type: "longblob", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.userId);
                    table.ForeignKey(
                        name: "fk_auth_user",
                        column: x => x.userId,
                        principalTable: "user",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "billcustomer",
                columns: table => new
                {
                    userId = table.Column<byte[]>(type: "binary(16)", fixedLength: true, maxLength: 16, nullable: false),
                    billId = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<sbyte>(type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.userId, x.billId })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "bill_bill_customer",
                        column: x => x.billId,
                        principalTable: "bill",
                        principalColumn: "billId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_billcustomer_user",
                        column: x => x.userId,
                        principalTable: "user",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "service_active_user_idx",
                table: "activatedservices",
                column: "serviceId");

            migrationBuilder.CreateIndex(
                name: "bill_bill_customer_idx",
                table: "billcustomer",
                column: "billId");

            migrationBuilder.CreateIndex(
                name: "service_bill_service_idx",
                table: "servicebill",
                column: "serviceId");

            migrationBuilder.CreateIndex(
                name: "phoneNumber_UNIQUE",
                table: "user",
                column: "phoneNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "activatedservices");

            migrationBuilder.DropTable(
                name: "auth");

            migrationBuilder.DropTable(
                name: "billcustomer");

            migrationBuilder.DropTable(
                name: "BillTelcoservices");

            migrationBuilder.DropTable(
                name: "servicebill");

            migrationBuilder.DropTable(
                name: "TelcoservicesUser");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "bill");

            migrationBuilder.DropTable(
                name: "telcoservices");
        }
    }
}
