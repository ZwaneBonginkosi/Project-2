using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace CurrencyExchange.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "CurrencyExchange");

            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ConversionHistory",
                schema: "CurrencyExchange",
                columns: table => new
                {
                    RatesHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Base = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false),
                    Amount = table.Column<double>(type: "double", precision: 15, nullable: false),
                    Target = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false),
                    TargetAmount = table.Column<double>(type: "double", precision: 15, nullable: false),
                    Rate = table.Column<double>(type: "double", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversionHistory", x => x.RatesHistoryId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CurrencyPair",
                schema: "CurrencyExchange",
                columns: table => new
                {
                    CurrencyPairId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Base = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false),
                    Target = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false),
                    Rate = table.Column<double>(type: "double", precision: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyPair", x => x.CurrencyPairId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConversionHistory",
                schema: "CurrencyExchange");

            migrationBuilder.DropTable(
                name: "CurrencyPair",
                schema: "CurrencyExchange");
        }
    }
}
