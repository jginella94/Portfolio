using Microsoft.EntityFrameworkCore;
using ObligatorioP3.Infraestructura.AccesoDatos;
using ObligatorioP3.Infraestructura.AccesoDatos.EF;
using ObligatorioP3.Infraestructura.EF.AccesoDatos;
using ObligatorioP3.LogicaAplicacion.CasosDeUso.AuditoriasHistorias;
using ObligatorioP3.LogicaAplicacion.CasosDeUso.Capitulos;
using ObligatorioP3.LogicaAplicacion.CasosDeUso.Categorias;
using ObligatorioP3.LogicaAplicacion.CasosDeUso.Historias;
using ObligatorioP3.LogicaAplicacion.CasosDeUso.Usuarios;
using ObligatorioP3.LogicaAplicacion.DTOs.AuditoriasHistorias;
using ObligatorioP3.LogicaAplicacion.DTOs.Capitulos;
using ObligatorioP3.LogicaAplicacion.DTOs.Categorias;
using ObligatorioP3.LogicaAplicacion.DTOs.Historias;
using ObligatorioP3.LogicaAplicacion.DTOs.Usuarios;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;

namespace ObligatorioP3.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddSession();

            //Repos
            builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
            builder.Services.AddScoped<IRepositorioHistoria, RepositorioHistoria>();
            builder.Services.AddScoped<IRepositorioCapitulo, RepositorioCapitulo>();
            builder.Services.AddScoped<IRepositorioOpcion, RepositorioOpcion>();
            builder.Services.AddScoped<IRepositorioLectura, RepositorioLectura>();
            builder.Services.AddScoped<IRepositorioCategoria, RepositorioCategoria>();
            builder.Services.AddScoped<IRepositorioAuditoriaHistoria, RepositorioAuditoriaHistoria>();
            builder.Services.AddScoped<IRepositorioBajaHistoriaAuditada, RepositorioHistoria>();

            //Casos de Uso
            builder.Services.AddScoped<IAlta<DtoUsuarioAlta>, AltaUsuario>();
            builder.Services.AddScoped<IAutenticar, AutenticarAdministrador>();
            builder.Services.AddScoped<IAlta<DtoHistoriaAlta>, AltaHistoria>();
            builder.Services.AddScoped<IListarTodos<DtoHistoriaListado>, ListarTodasLasHistorias>();
            builder.Services.AddScoped<IVer<DtoHistoriaListado>, VerHistoria>();
            builder.Services.AddScoped<IEditar<DtoHistoriaEdicion>, EditarHistoria>();
            builder.Services.AddScoped<IBorrar<DtoHistoriaListado>, BorrarHistoria>();
            builder.Services.AddScoped<IEditar<DtoCapituloInicialEdicion>, EditarCapituloInicial>();
            builder.Services.AddScoped<IListarTodos<DtoCategoria>, ListarTodasLasCategorias>();
            builder.Services.AddScoped<IVer<DtoHistoriaEdicion>, VerHistoriaEdicion>();
            builder.Services.AddScoped<IVer<DtoCapituloInicialEdicion>, VerCapituloInicial>();
            builder.Services.AddScoped<IEditar<DtoHistoriaEdicion>, EditarHistoria>();
            builder.Services.AddScoped<IEditar<DtoCapituloInicialEdicion>, EditarCapituloInicial>();
            builder.Services.AddScoped<IAlta<DtoCapituloAgregar>, AltaCapitulo>();
            builder.Services.AddScoped<IVer<DtoCapitulosParaAlta>, VerCapitulosParaAlta>();
            builder.Services.AddScoped<IRegistrarAuditoriaHistoria, RegistrarAuditoriaHistoria>();
            builder.Services.AddScoped<IConsultarAuditorias<DtoAuditoriaHistoria>, ConsultarAuditoriasHistoria>();
            builder.Services.AddScoped<IListarTodos<DtoAdministradorFiltroAuditoria>, ListarAdministradoresAuditoria>();
            builder.Services.AddScoped<IBorrarHistoriaAuditada, BorrarHistoria>();

            //Precarga
            builder.Services.AddScoped<SeedData>();

            builder.Services.AddDbContext<ObligatorioP3Context>(
                option => option.UseSqlServer(builder.Configuration.GetConnectionString("ObligatorioP3DB")));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                using (var scope = app.Services.CreateScope())
                {
                    var seedData = scope.ServiceProvider.GetRequiredService<SeedData>();
                    seedData.Seed();
                }
            }

            app.UseExceptionHandler("/Home/Error");

            if (!app.Environment.IsDevelopment())
                app.UseHsts();


            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
