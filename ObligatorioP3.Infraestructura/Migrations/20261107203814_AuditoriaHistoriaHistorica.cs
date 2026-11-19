using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObligatorioP3.Infraestructura.Migrations
{
    /// <inheritdoc />
    public partial class AuditoriaHistoriaHistorica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "HistoriaId",
                table: "AuditoriasHistoria",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "HistoriaIdHistorico",
                table: "AuditoriasHistoria",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"
                UPDATE AuditoriasHistoria
                SET HistoriaIdHistorico = HistoriaId
                WHERE HistoriaIdHistorico = 0
                  AND HistoriaId IS NOT NULL;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HistoriaIdHistorico",
                table: "AuditoriasHistoria");

            migrationBuilder.AlterColumn<int>(
                name: "HistoriaId",
                table: "AuditoriasHistoria",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
