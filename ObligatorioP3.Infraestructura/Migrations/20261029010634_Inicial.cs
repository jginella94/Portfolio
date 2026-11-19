using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObligatorioP3.Infraestructura.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCompleto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditoriasHistoria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistoriaId = table.Column<int>(type: "int", nullable: false),
                    AdministradorId = table.Column<int>(type: "int", nullable: false),
                    Accion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditoriasHistoria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditoriasHistoria_Usuarios_AdministradorId",
                        column: x => x.AdministradorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Capitulos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Texto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    HistoriaId = table.Column<int>(type: "int", nullable: true),
                    VecesAlcanzado = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Capitulos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Historias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sinopsis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CapituloInicialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Historias_Capitulos_CapituloInicialId",
                        column: x => x.CapituloInicialId,
                        principalTable: "Capitulos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Opciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Texto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DestinoId = table.Column<int>(type: "int", nullable: false),
                    CapituloIntermedioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Opciones_Capitulos_CapituloIntermedioId",
                        column: x => x.CapituloIntermedioId,
                        principalTable: "Capitulos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Opciones_Capitulos_DestinoId",
                        column: x => x.DestinoId,
                        principalTable: "Capitulos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HistoriaCategoria",
                columns: table => new
                {
                    HistoriaId = table.Column<int>(type: "int", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoriaCategoria", x => new { x.HistoriaId, x.CategoriaId });
                    table.ForeignKey(
                        name: "FK_HistoriaCategoria_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HistoriaCategoria_Historias_HistoriaId",
                        column: x => x.HistoriaId,
                        principalTable: "Historias",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Lecturas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    HistoriaId = table.Column<int>(type: "int", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFinalizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinalAlcanzadoId = table.Column<int>(type: "int", nullable: true),
                    CapituloActualId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lecturas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lecturas_Capitulos_CapituloActualId",
                        column: x => x.CapituloActualId,
                        principalTable: "Capitulos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Lecturas_Capitulos_FinalAlcanzadoId",
                        column: x => x.FinalAlcanzadoId,
                        principalTable: "Capitulos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Lecturas_Historias_HistoriaId",
                        column: x => x.HistoriaId,
                        principalTable: "Historias",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Lecturas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriasHistoria_AdministradorId",
                table: "AuditoriasHistoria",
                column: "AdministradorId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriasHistoria_HistoriaId",
                table: "AuditoriasHistoria",
                column: "HistoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Capitulos_HistoriaId",
                table: "Capitulos",
                column: "HistoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoriaCategoria_CategoriaId",
                table: "HistoriaCategoria",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Historias_CapituloInicialId",
                table: "Historias",
                column: "CapituloInicialId");

            migrationBuilder.CreateIndex(
                name: "IX_Lecturas_CapituloActualId",
                table: "Lecturas",
                column: "CapituloActualId");

            migrationBuilder.CreateIndex(
                name: "IX_Lecturas_FinalAlcanzadoId",
                table: "Lecturas",
                column: "FinalAlcanzadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Lecturas_HistoriaId",
                table: "Lecturas",
                column: "HistoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Lecturas_UsuarioId",
                table: "Lecturas",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Opciones_CapituloIntermedioId",
                table: "Opciones",
                column: "CapituloIntermedioId");

            migrationBuilder.CreateIndex(
                name: "IX_Opciones_DestinoId",
                table: "Opciones",
                column: "DestinoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditoriasHistoria_Historias_HistoriaId",
                table: "AuditoriasHistoria",
                column: "HistoriaId",
                principalTable: "Historias",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Capitulos_Historias_HistoriaId",
                table: "Capitulos",
                column: "HistoriaId",
                principalTable: "Historias",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Capitulos_Historias_HistoriaId",
                table: "Capitulos");

            migrationBuilder.DropTable(
                name: "AuditoriasHistoria");

            migrationBuilder.DropTable(
                name: "HistoriaCategoria");

            migrationBuilder.DropTable(
                name: "Lecturas");

            migrationBuilder.DropTable(
                name: "Opciones");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Historias");

            migrationBuilder.DropTable(
                name: "Capitulos");
        }
    }
}
