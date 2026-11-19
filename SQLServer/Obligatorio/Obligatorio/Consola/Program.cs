using Dominio;

namespace Consola
{
    internal class Program
    {
        static Sistema miSistema;
        static void Main(string[] args)
        {
            miSistema = new Sistema();
            string opcion = "";

            while (opcion != "0")
            {
                MostrarMenu();
                Console.Write("Ingrese una opcion -> ");
                opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        ListarUsuarios();
                        PressToContinue();
                        break;
                    case "2":
                        BuscarPagosPorEmail();
                        PressToContinue();
                        break;
                    case "3":
                        AltaUsuario();
                        PressToContinue();
                        break;
                    case "4":
                        ListarUsuariosPorEquipo();
                        PressToContinue();
                        break;
                    case "0":
                        Console.WriteLine("Saliendo...");
                        break;
                    default:
                        MostrarError("ERROR: Opcion inválida");
                        PressToContinue();
                        break;
                }
            }
        }

        static void MostrarMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("****** MENU ******");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("1 - Listado de todos los usuarios");
            Console.WriteLine("2 - Buscar pagos por email de usario");
            Console.WriteLine("3 - Dar de alta un usuario");
            Console.WriteLine("4 - Listar usuarios por equipo");
            Console.WriteLine("0 - Salir");
        }

        static void ListarUsuarios()
        {
            try
            {
                if (miSistema.Usuarios.Count == 0) throw new Exception("No hay usuarios en el sistema");
                MostrarCabezal("Lista de usuarios:");
                foreach (Usuario u in miSistema.Usuarios)
                {
                    Console.WriteLine(u);
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
            }
        }

        static void BuscarPagosPorEmail()
        {
            try
            {
                string email = PedirPalabras("Ingrese el email del usuario: ", "El email no puede ser vacio");
                List<Pago> pagos = miSistema.BuscarPagosPorEmail(email);
                MostrarCabezal("Pagos del Usuario:");
                foreach(Pago pago in pagos)
                {
                    Console.WriteLine(pago);
                }
            } 
            catch(Exception ex) 
            {
                    MostrarError(ex.Message);
            }
        }

        static void AltaUsuario()
        {
            try
            {
                string nom = PedirPalabras("Ingrese el nombre: ", "El nombre no puede ser vacio");
                string ape = PedirPalabras("Ingrese el apellido: ", "El apellido no puede ser vacio");
                string pas = PedirPalabras("Ingrese la contraseña: ", "La contraseña no puede ser vacia");
                if (pas.Length != 8) throw new Exception("La contraseña debe tener 8 caracteres");
                string eNombre = PedirPalabras("Ingrese nombre del equipo: ", "El nombre de equipo no puede ser vacio");

                Equipo equipo = miSistema.BuscarEquipoPorNombre(eNombre);
                DateTime fecha = PedirFecha("Ingrese la fecha de incorporacion: ");
 
                miSistema.CrearUsuario(new Usuario(nom, ape, pas, equipo, fecha));
                MostrarExito("El usuario fue creado correctamente");
            }
            catch(Exception ex)
            {
                MostrarError(ex.Message);
            }
        }

        static void ListarUsuariosPorEquipo()
        {
            try
            {
                string eNombre = PedirPalabras("Ingrese nombre del equipo: ", "El nombre de equipo no puede ser vacio");
                Equipo equipo = miSistema.BuscarEquipoPorNombre(eNombre);
                if (equipo == null) throw new Exception($"No se encotro un equipo con el nombre {eNombre}");
                List<Usuario> usus = miSistema.ListarUsuariosPorEquipo(equipo);
                MostrarCabezal($"Usuarios del Equipo {equipo.Nombre}");
                foreach(Usuario usuario in usus )
                {
                    Console.WriteLine(usuario);
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
            }
        }

        #region FuncionesAuxiliares
        static void PressToContinue()
        {
            Console.WriteLine("Presione una tecla para continuar");
            Console.ReadKey();
        }

        static string PedirPalabras(string mensaje, string error)
        {
            Console.Write(mensaje);
            string dato = Console.ReadLine();
            if (string.IsNullOrEmpty(dato)) throw new Exception(error);
            return dato;
        }

        static DateTime PedirFecha(string mensaje)
        {
            bool exito = false;
            DateTime fecha = new DateTime();
            while (!exito)
            {
                Console.Write(mensaje + " [DD/MM/YYYY]:");
                exito = DateTime.TryParse(Console.ReadLine(), out fecha);

                if (!exito) MostrarError("ERROR: Debe ingresar una fecha en formato DD/MM/YYYY");
            }
            return fecha;
        }

        static void MostrarError(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(mensaje);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        static void MostrarExito(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(mensaje);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        static void MostrarCabezal(string mensaje)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(mensaje);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
        }
        #endregion
    }
}
