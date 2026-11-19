using ObligatorioP3.LogicaAplicacion.DTOs.Usuarios;
using ObligatorioP3.LogicaNegocio.Entidades;
using ObligatorioP3.LogicaNegocio.Vo;

namespace ObligatorioP3.LogicaAplicacion.Mapper
{
    public static class UsuarioMapper
    {
        public static Usuario DesdeDto(DtoUsuarioAlta dtoUsuario)
        {
            if (dtoUsuario.Rol == "Administrador")
            {
                return new Administrador(
                    
                    new VoNombreCompleto(dtoUsuario.NombreCompleto),                    
                    new VoNombreUsuario(dtoUsuario.NombreUsuario),
                    new VoPass(dtoUsuario.Password),
                    new VoEmail(dtoUsuario.Email)
                );
            }
            else if (dtoUsuario.Rol == "Lector")
            {
                return new Lector(
                    new VoNombreCompleto(dtoUsuario.NombreCompleto),
                    new VoNombreUsuario(dtoUsuario.NombreUsuario),
                    new VoPass(dtoUsuario.Password),
                    new VoEmail(dtoUsuario.Email)
                );
            }
            else
            {
                throw new ArgumentException("Rol no válido. Debe ser 'Administrador' o 'Lector'.");
            }
        }

        public static DtoUsuarioListado DesdeEntidad(Usuario usuario)
        {
            
            return new DtoUsuarioListado(
                Id: usuario.Id,
                NombreCompleto: usuario.NombreCompleto.Value,
                Email: usuario.Email.Value,
                UserName: usuario.NombreUsuario.Value,
                Rol: usuario is Administrador ? "Administrador" : "Lector"
                
            );
        }

        public static IEnumerable<DtoUsuarioListado> ListadoDto(IEnumerable<Usuario> usuarios)
        {
            List<DtoUsuarioListado> aux = new List<DtoUsuarioListado>();
            foreach (Usuario usuario in usuarios)
            {
                aux.Add(DesdeEntidad(usuario));
            }
            return aux;
        }
    }
}