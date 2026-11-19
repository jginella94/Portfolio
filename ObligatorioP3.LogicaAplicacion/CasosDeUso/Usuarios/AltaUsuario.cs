using ObligatorioP3.LogicaAplicacion.DTOs.Usuarios;
using ObligatorioP3.LogicaNegocio.Entidades;
using ObligatorioP3.LogicaNegocio.Excepciones;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;
using ObligatorioP3.LogicaNegocio.Vo;

namespace ObligatorioP3.LogicaAplicacion.CasosDeUso.Usuarios
{
    public class AltaUsuario : IAlta<DtoUsuarioAlta>
    {
        private readonly IRepositorioUsuario _repositorioUsuario;

        public AltaUsuario(IRepositorioUsuario repositorioUsuario)
        {
            _repositorioUsuario = repositorioUsuario;
        }

        public int Ejecutar(DtoUsuarioAlta dto)
        {
            if (dto is null)
                throw new LogicaNegocioException("Debe ingresar los datos del usuario.");

            VoNombreCompleto nombreCompleto = new VoNombreCompleto(dto.NombreCompleto);
            VoNombreUsuario nombreUsuario = new VoNombreUsuario(dto.NombreUsuario);
            VoEmail email = new VoEmail(dto.Email);
            VoPass password = new VoPass(dto.Password);

            if (_repositorioUsuario.GetAll().Any(u =>
                string.Equals(u.NombreUsuario.Value, nombreUsuario.Value, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(u.Email.Value, email.Value, StringComparison.OrdinalIgnoreCase)))
            {
                throw new LogicaNegocioException("El nombre de usuario o el email ya están registrados.");
            }

            Usuario usuario = dto.Rol?.Trim().ToLowerInvariant() switch
            {
                "administrador" => new Administrador(nombreCompleto, nombreUsuario, password, email),
                "usuario" => new Lector(nombreCompleto, nombreUsuario, password, email),
                _ => throw new LogicaNegocioException("Debe seleccionar un rol válido: Administrador o Usuario.")
            };

            return _repositorioUsuario.Add(usuario);
        }
    }
}
