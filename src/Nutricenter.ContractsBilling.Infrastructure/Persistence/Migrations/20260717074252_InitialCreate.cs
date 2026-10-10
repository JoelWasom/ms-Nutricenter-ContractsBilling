using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nutricenter.ContractsBilling.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    ContractId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PacienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumeroContrato = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    FechaContrato = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaFin = table.Column<DateOnly>(type: "date", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.ContractId);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTypes",
                columns: table => new
                {
                    ServiceTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NombreMicroservicio = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    BaseUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Endpoint = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    IsService = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTypes", x => x.ServiceTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ContractedServices",
                columns: table => new
                {
                    ContractServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContractId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(512)", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DuracionDias = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractedServices", x => x.ContractServiceId);
                    table.ForeignKey(
                        name: "FK_ContractedServices_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "ContractId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractedServices_ContractId",
                table: "ContractedServices",
                column: "ContractId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractedServices");

            migrationBuilder.DropTable(
                name: "ServiceTypes");

            migrationBuilder.DropTable(
                name: "Contracts");
        }
    }
}
