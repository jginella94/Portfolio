using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Sistema
    {
        private List<TipoGasto> _tiposGastos = new List<TipoGasto>();
        private List<Pago> _pagos = new List<Pago>();
        private List<Usuario> _usuarios = new List<Usuario>();
        private List<Equipo> _equipos = new List<Equipo>();
        public static Sistema s_instancia;

        public List<TipoGasto> TipoGasto
        {
            get { return _tiposGastos; }
        }

        private Sistema()
        {
            PrecargarEquipos();
            PrecargarUsuarios();
            PrecargarTipoGastos();
            PrecargarPagos();
        }

        public static Sistema Instancia
        {
            get
            {
                if (s_instancia == null) s_instancia = new Sistema();
                return s_instancia;
            }
        }



        public List<Usuario> Usuarios
        {
            get { return _usuarios; }
        }

        #region Precargas
        //Equipos: 4
        //Usuarios: 22
        //TipoGastos: 10
        //Pagos: 42 pagos totales, 25 recurrentes(5 de ellos deben estar pagados totalmente)

        private void PrecargarEquipos()
        {
            CrearEquipo(new Equipo("Back-end"));
            CrearEquipo(new Equipo("Front-end"));
            CrearEquipo(new Equipo("Data"));
            CrearEquipo(new Equipo("Mobile"));
        }

        private void PrecargarUsuarios()
        {
            DateTime hoy = DateTime.Now.Date;
            string pass = "Passw0rd!";

            CrearUsuario(new Usuario("Ana", "García", pass, _equipos[0], hoy.AddMonths(-18), Rol.EMPLEADO));
            CrearUsuario(new Usuario("Bruno", "Pérez", pass, _equipos[1], hoy.AddMonths(-15), Rol.EMPLEADO));
            CrearUsuario(new Usuario("Camila", "Silva", pass, _equipos[2], hoy.AddMonths(-12), Rol.EMPLEADO));
            CrearUsuario(new Usuario("Diego", "López", pass, _equipos[3], hoy.AddMonths(-10), Rol.EMPLEADO));
            CrearUsuario(new Usuario("Elena", "Rodríguez", pass, _equipos[0], hoy.AddMonths(-9), Rol.EMPLEADO));
            CrearUsuario(new Usuario("Fabio", "Suárez", pass, _equipos[1], hoy.AddMonths(-8), Rol.EMPLEADO));
            CrearUsuario(new Usuario("Gina", "Ramos", pass, _equipos[2], hoy.AddMonths(-8), Rol.EMPLEADO));
            CrearUsuario(new Usuario("Hugo", "Santos", pass, _equipos[3], hoy.AddMonths(-7), Rol.EMPLEADO));
            CrearUsuario(new Usuario("Irene", "Costa", pass, _equipos[0], hoy.AddMonths(-7), Rol.EMPLEADO));
            CrearUsuario(new Usuario("Juan", "Martínez", pass, _equipos[1], hoy.AddMonths(-6), Rol.GERENTE));
            CrearUsuario(new Usuario("Karla", "Mendez", pass, _equipos[2], hoy.AddMonths(-6), Rol.GERENTE));
            CrearUsuario(new Usuario("Lucas", "Fernández", pass, _equipos[3], hoy.AddMonths(-5), Rol.EMPLEADO));
            CrearUsuario(new Usuario("Mara", "Vega", pass, _equipos[0], hoy.AddMonths(-5), Rol.EMPLEADO));
            CrearUsuario(new Usuario("Nico", "Sosa", pass, _equipos[1], hoy.AddMonths(-4), Rol.EMPLEADO));
            CrearUsuario(new Usuario("Ana", "Gar", pass, _equipos[2], hoy.AddMonths(-4), Rol.EMPLEADO));
            CrearUsuario(new Usuario("Pablo", "Núñez", pass, _equipos[3], hoy.AddMonths(-3), Rol.EMPLEADO));
            CrearUsuario(new Usuario("Quimey", "Ortiz", pass, _equipos[0], hoy.AddMonths(-3), Rol.EMPLEADO));
            CrearUsuario(new Usuario("Rocío", "Luna", pass, _equipos[1], hoy.AddMonths(-2), Rol.EMPLEADO));
            CrearUsuario(new Usuario("Sergio", "Aguiar", pass, _equipos[2], hoy.AddMonths(-2), Rol.EMPLEADO));
            CrearUsuario(new Usuario("Ana", "García", pass, _equipos[3], hoy.AddMonths(-1), Rol.EMPLEADO));
            CrearUsuario(new Usuario("Ulises", "Quiroga", pass, _equipos[0], hoy.AddMonths(-1), Rol.EMPLEADO));
            CrearUsuario(new Usuario("Valen", "Ibarra", pass, _equipos[1], hoy.AddDays(-15), Rol.EMPLEADO));
            CrearUsuario(new Usuario("Rodri", "Cornelius", pass, _equipos[0], hoy.AddMonths(-18), Rol.EMPLEADO));
        }

        private void PrecargarTipoGastos()
        {
            CrearTipoGasto(new TipoGasto("Alquiler", "Renta mensual de oficinas"));
            CrearTipoGasto(new TipoGasto("Luz", "Energía eléctrica"));
            CrearTipoGasto(new TipoGasto("Agua", "Suministro de agua potable"));
            CrearTipoGasto(new TipoGasto("Internet", "Conectividad y fibra óptica"));
            CrearTipoGasto(new TipoGasto("Limpieza", "Servicio de higiene y mantenimiento"));
            CrearTipoGasto(new TipoGasto("Servicios Cloud", "AWS, Azure o GCP"));
            CrearTipoGasto(new TipoGasto("Licencias", "Software, herramientas y suscripciones"));
            CrearTipoGasto(new TipoGasto("Marketing", "Publicidad, redes y campañas digitales"));
            CrearTipoGasto(new TipoGasto("Mobiliario", "Sillas, escritorios y equipamiento"));
            CrearTipoGasto(new TipoGasto("Seguros", "Pólizas de seguro y cobertura"));
            CrearTipoGasto(new TipoGasto("ParaPruebasdeEliminar", "Test"));
        }

        private void PrecargarPagos()
        {
            DateTime hoy = DateTime.Now.Date;

            // ===========================
            // 20 RECURRENTES ACTIVOS
            // fechaFin en el futuro
            // ===========================
            CrearPagoRecurrente(new Recurrente(MetodoPago.CREDITO, _tiposGastos[0], _usuarios[0], "Recurrente activo #1", 1000, hoy.AddMonths(-6), hoy.AddMonths(3)));
            CrearPagoRecurrente(new Recurrente(MetodoPago.DEBITO, _tiposGastos[1], _usuarios[1], "Recurrente activo #2", 1100, hoy.AddMonths(-5), hoy.AddMonths(4)));
            CrearPagoRecurrente(new Recurrente(MetodoPago.EFECTIVO, _tiposGastos[2], _usuarios[2], "Recurrente activo #3", 950, hoy.AddMonths(-4), hoy.AddMonths(2)));
            CrearPagoRecurrente(new Recurrente(MetodoPago.CREDITO, _tiposGastos[3], _usuarios[3], "Recurrente activo #4", 1200, hoy.AddMonths(-8), hoy.AddMonths(1)));
            CrearPagoRecurrente(new Recurrente(MetodoPago.DEBITO, _tiposGastos[4], _usuarios[4], "Recurrente activo #5", 1300, hoy.AddMonths(-10), hoy.AddMonths(2)));
            CrearPagoRecurrente(new Recurrente(MetodoPago.EFECTIVO, _tiposGastos[5], _usuarios[5], "Recurrente activo #6", 1050, hoy.AddMonths(-7), hoy.AddMonths(5)));
            CrearPagoRecurrente(new Recurrente(MetodoPago.CREDITO, _tiposGastos[6], _usuarios[6], "Recurrente activo #7", 1150, hoy.AddMonths(-6), hoy.AddMonths(6)));
            CrearPagoRecurrente(new Recurrente(MetodoPago.DEBITO, _tiposGastos[7], _usuarios[7], "Recurrente activo #8", 1400, hoy.AddMonths(-9), null));
            CrearPagoRecurrente(new Recurrente(MetodoPago.EFECTIVO, _tiposGastos[8], _usuarios[8], "Recurrente activo #9", 1250, hoy.AddMonths(-3), hoy.AddMonths(4)));
            CrearPagoRecurrente(new Recurrente(MetodoPago.CREDITO, _tiposGastos[9], _usuarios[9], "Recurrente activo #10", 980, hoy.AddMonths(-2), hoy.AddMonths(2)));
            CrearPagoRecurrente(new Recurrente(MetodoPago.DEBITO, _tiposGastos[0], _usuarios[10], "Recurrente activo #11", 1010, hoy.AddMonths(-6), hoy.AddMonths(1)));
            CrearPagoRecurrente(new Recurrente(MetodoPago.EFECTIVO, _tiposGastos[1], _usuarios[11], "Recurrente activo #12", 1180, hoy.AddMonths(-5), hoy.AddMonths(3)));
            CrearPagoRecurrente(new Recurrente(MetodoPago.CREDITO, _tiposGastos[2], _usuarios[12], "Recurrente activo #13", 1110, hoy.AddMonths(-4), hoy.AddMonths(5)));
            CrearPagoRecurrente(new Recurrente(MetodoPago.DEBITO, _tiposGastos[3], _usuarios[13], "Recurrente activo #14", 1090, hoy.AddMonths(-8), hoy.AddMonths(2)));
            CrearPagoRecurrente(new Recurrente(MetodoPago.EFECTIVO, _tiposGastos[4], _usuarios[14], "Recurrente activo #15", 1190, hoy.AddMonths(-7), null));
            CrearPagoRecurrente(new Recurrente(MetodoPago.CREDITO, _tiposGastos[5], _usuarios[15], "Recurrente activo #16", 1340, hoy.AddMonths(-9), hoy.AddMonths(4)));
            CrearPagoRecurrente(new Recurrente(MetodoPago.DEBITO, _tiposGastos[6], _usuarios[16], "Recurrente activo #17", 1070, hoy.AddMonths(-3), hoy.AddMonths(1)));
            CrearPagoRecurrente(new Recurrente(MetodoPago.EFECTIVO, _tiposGastos[7], _usuarios[17], "Recurrente activo #18", 1220, hoy.AddMonths(-6), hoy.AddMonths(2)));
            CrearPagoRecurrente(new Recurrente(MetodoPago.CREDITO, _tiposGastos[8], _usuarios[18], "Recurrente activo #19", 1280, hoy.AddMonths(-5), null));
            CrearPagoRecurrente(new Recurrente(MetodoPago.DEBITO, _tiposGastos[9], _usuarios[19], "Recurrente activo #20", 1130, hoy.AddMonths(-4), hoy.AddMonths(4)));
            CrearPagoRecurrente(new Recurrente(MetodoPago.DEBITO, _tiposGastos[9], _usuarios[19], "Recurrente activo #20", 1130, hoy.AddMonths(-4), hoy.AddMonths(4)));


            // ===========================
            // 5 RECURRENTES FINALIZADOS
            // fechaFin en el pasado
            // ===========================
            CrearPagoRecurrente(new Recurrente(MetodoPago.CREDITO, _tiposGastos[5], _usuarios[5], "Recurrente finalizado #1", 800, hoy.AddMonths(-18), hoy.AddMonths(-3)));
            CrearPagoRecurrente(new Recurrente(MetodoPago.DEBITO, _tiposGastos[6], _usuarios[6], "Recurrente finalizado #2", 750, hoy.AddMonths(-16), hoy.AddMonths(-2)));
            CrearPagoRecurrente(new Recurrente(MetodoPago.EFECTIVO, _tiposGastos[7], _usuarios[7], "Recurrente finalizado #3", 700, hoy.AddMonths(-15), hoy.AddMonths(-1)));
            CrearPagoRecurrente(new Recurrente(MetodoPago.CREDITO, _tiposGastos[8], _usuarios[8], "Recurrente finalizado #4", 900, hoy.AddMonths(-20), hoy.AddMonths(-5)));
            CrearPagoRecurrente(new Recurrente(MetodoPago.DEBITO, _tiposGastos[9], _usuarios[9], "Recurrente finalizado #5", 650, hoy.AddMonths(-12), hoy.AddMonths(-4)));

            // ===========================
            // 17 PAGOS ÚNICOS
            // ===========================
            CrearPagoUnico(new Unico(MetodoPago.CREDITO, _tiposGastos[0], _usuarios[10], "Compra única #1", 600, hoy.AddDays(-15), 10001));
            CrearPagoUnico(new Unico(MetodoPago.DEBITO, _tiposGastos[1], _usuarios[11], "Compra única #2", 700, hoy.AddDays(-25), 10002));
            CrearPagoUnico(new Unico(MetodoPago.EFECTIVO, _tiposGastos[2], _usuarios[12], "Compra única #3", 550, hoy.AddDays(-40), 10003));
            CrearPagoUnico(new Unico(MetodoPago.CREDITO, _tiposGastos[3], _usuarios[13], "Compra única #4", 900, hoy.AddDays(-50), 10004));
            CrearPagoUnico(new Unico(MetodoPago.DEBITO, _tiposGastos[4], _usuarios[14], "Compra única #5", 1000, hoy.AddDays(-60), 10005));
            CrearPagoUnico(new Unico(MetodoPago.EFECTIVO, _tiposGastos[5], _usuarios[15], "Compra única #6", 650, hoy.AddDays(-70), 10006));
            CrearPagoUnico(new Unico(MetodoPago.CREDITO, _tiposGastos[6], _usuarios[16], "Compra única #7", 800, hoy.AddDays(-90), 10007));
            CrearPagoUnico(new Unico(MetodoPago.DEBITO, _tiposGastos[7], _usuarios[17], "Compra única #8", 750, hoy.AddDays(-100), 10008));
            CrearPagoUnico(new Unico(MetodoPago.EFECTIVO, _tiposGastos[8], _usuarios[18], "Compra única #9", 950, hoy.AddDays(-110), 10009));
            CrearPagoUnico(new Unico(MetodoPago.CREDITO, _tiposGastos[9], _usuarios[19], "Compra única #10", 880, hoy.AddDays(-120), 10010));
            CrearPagoUnico(new Unico(MetodoPago.DEBITO, _tiposGastos[0], _usuarios[20], "Compra única #11", 720, hoy.AddDays(-130), 10011));
            CrearPagoUnico(new Unico(MetodoPago.EFECTIVO, _tiposGastos[1], _usuarios[21], "Compra única #12", 640, hoy.AddDays(-140), 10012));
            CrearPagoUnico(new Unico(MetodoPago.CREDITO, _tiposGastos[2], _usuarios[0], "Compra única #13", 990, hoy.AddDays(-45), 10013));
            CrearPagoUnico(new Unico(MetodoPago.DEBITO, _tiposGastos[3], _usuarios[1], "Compra única #14", 560, hoy.AddDays(-35), 10014));
            CrearPagoUnico(new Unico(MetodoPago.EFECTIVO, _tiposGastos[4], _usuarios[2], "Compra única #15", 610, hoy.AddDays(-20), 10015));
            CrearPagoUnico(new Unico(MetodoPago.CREDITO, _tiposGastos[5], _usuarios[3], "Compra única #16", 730, hoy.AddDays(-10), 10016));
            CrearPagoUnico(new Unico(MetodoPago.DEBITO, _tiposGastos[6], _usuarios[4], "Compra única #17", 845, hoy.AddDays(-5), 10017));
            CrearPagoUnico(new Unico(MetodoPago.CREDITO, _tiposGastos[0], _usuarios[10], "Compra única #1", 15000, hoy.AddDays(-2), 10018));
        }

        #endregion

        #region Creaciones
        public void CrearEquipo(Equipo equipo)
        {
            if (equipo == null) throw new Exception("El equipo no puede ser nulo.");
            equipo.Validar();
            _equipos.Add(equipo);
        }

        public void CrearUsuario(Usuario usuario)
        {
            if (usuario == null) throw new Exception("El usuario no puede ser nulo.");
            usuario.Validar();
            GenerarEmail(usuario);
            _usuarios.Add(usuario);
        }

        public void CrearTipoGasto(TipoGasto tipoGasto)
        {
            if (tipoGasto == null) throw new Exception("El tipo de gasto no puede ser nulo.");
            tipoGasto.Validar();
            _tiposGastos.Add(tipoGasto);
        }

        public void CrearPagoUnico(Unico unico)
        {
            if (unico == null) throw new Exception("El pago unico no puede ser nulo.");
            unico.Validar();
            _pagos.Add(unico);
        }

        public void CrearPagoRecurrente(Recurrente recurrente)
        {
            if (recurrente == null) throw new Exception("El pago recurrente no puede ser nulo.");
            recurrente.Validar();
            _pagos.Add(recurrente);
        }
        #endregion

        

        /// <summary>
        /// Valida que el email que se agrega al usuario sea unico y altera el email de ser necesario.
        /// </summary>
        /// <param name="u">recibe un Usuario al que se le agrega el email</param>
        private void GenerarEmail(Usuario u)
        {
            string emailPrimario = u.GenerarSubDominioPrimario();
            int i = 1;
            while (_usuarios.Contains(u))
            {
                string emailCompuesto = $"{emailPrimario}{i}";
                u.Email = emailCompuesto;
                i++;
            }
        }


        /// <summary>
        /// busca usuarios a partir de un email
        /// </summary>
        /// <param name="email">email de usuario</param>
        /// <returns>devuelve objeto Usuario</returns>
        public Usuario BuscarUsuario(string email)
        {
            Usuario encontrado = null;
            int i = 0;
            while (encontrado == null && i < _usuarios.Count)
            {
                if (_usuarios[i].Email == email) encontrado = _usuarios[i];
                i++;
            }
            return encontrado;
        }


        
        public List<Pago> BuscarPagosPorEmailMesActual(string email)
        {
            Usuario u = BuscarUsuario(email);
            if (u == null) throw new Exception("No se encontro el usuario");
            List<Pago> pagos = new List<Pago>();
            foreach (Pago p in _pagos)
            {
                if (u.Equals(p.Usuario) && p.EsMesCorriente()) pagos.Add(p);
            }
            if (pagos.Count == 0) throw new Exception("El usuario no tiene pagos registrados");
            pagos.Sort();
            return pagos;
        }


     
        /// <summary>
        /// lista todos los usuarios del equipo
        /// </summary>
        /// <param name="e">nombre de equipo</param>
        /// <returns>lista de usuarios</returns>
        public List<Usuario> ListarUsuariosPorEquipo(Equipo e)
        {
            List<Usuario> resultado = new List<Usuario>();
            foreach (Usuario u in _usuarios)
            {
                if (u.EsDeEquipo(e)) resultado.Add(u);
            }
            if (resultado.Count == 0) throw new Exception("No hay usuarios en ese equipo");
            return resultado;
        }

        public Usuario Login(string email, string pass)
        {
            Usuario buscado = null;
            int i = 0;
            while (buscado == null && i < _usuarios.Count)
            {
                if (_usuarios[i].Loguea(email, pass)) buscado = _usuarios[i];
                i++;
            }

            return buscado;
        }

        public TipoGasto BuscarTipoGasto(string n)
        {
            TipoGasto encontrado = null;
            int i = 0;
            while (encontrado == null && i < _tiposGastos.Count)
            {
                if (_tiposGastos[i].Nombre == n) encontrado = _tiposGastos[i];
                i++;
            }
            return encontrado;
        }

        public double CalcularGastoTotalDelMesActual(Usuario u)
        {
            List<Pago> pagos = BuscarPagosPorEmailMesActual(u.Email);

            double total = 0;

            foreach (Pago p in pagos)
            {
                total += p.CalcularGasto();
            }

            return total;
        }

        public List<Pago> BuscarPagosPorEmailFecha(Usuario u, int m, int a)
        {
            List<Pago> pagos = new List<Pago>();
            foreach (Pago p in _pagos)
            {
                if (u.Equals(p.Usuario) && p.CorrespondeMesAnio(m, a)) pagos.Add(p);
            }

            return pagos;
        }

        public List<Pago> ListarPagosPorEquipo(List<Usuario> usuarios, int m, int a)
        {
            List<Pago> pagos = new List<Pago>();

            foreach (Usuario u in usuarios)
            {
                pagos.AddRange(BuscarPagosPorEmailFecha(u, m, a));
            }

            if (pagos.Count == 0) throw new Exception("El Equipo no tiene pagos registrados");
            pagos.Sort();
            return pagos;
        }

        public void EliminarTipoGasto(TipoGasto tipoGasto)
        {
            TienePagoAsociado(tipoGasto);
            bool algo = _tiposGastos.Remove(tipoGasto);
            Console.WriteLine(_tiposGastos);
            Console.WriteLine(algo);

        }

        private void TienePagoAsociado(TipoGasto tipoGasto)
        {
            foreach (Pago p in _pagos)
            {
                if (p.TipoGasto.Equals(tipoGasto)) throw new Exception("El Tipo de Gasto se esta utilizando en al menos un pago");
            }
        }
    }


}
