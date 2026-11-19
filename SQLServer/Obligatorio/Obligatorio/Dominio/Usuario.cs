using Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Usuario : IValidable, IComparable<Usuario>
    {
        private string _nombre;
        private string _apellido;
        private string _password;
        private Equipo _equipo;
        private DateTime _fechaIncorporacion;
        private string _email;
        public Rol Rol {  get; set; }

        public Usuario(string nombre, string apellido, string password, Equipo equipo, DateTime fechaIncorporacion, Rol rol)
        {
            _nombre = nombre;
            _apellido = apellido;
            _password = password;
            _equipo = equipo;
            _fechaIncorporacion = fechaIncorporacion;
            Rol = rol;
        }

        public string Email
        {
            get { return _email; }
            set { _email = $"{value}@laempresa.com"; }
        }
        public string Nombre
        {
            get { return _nombre; }

        }
            public string Apellido
        {
            get { return _apellido; }
            
        }

        public Equipo Equipo
        {
            get { return _equipo; }
        }

        public DateTime FechaIncorporacion
        {
            get { return _fechaIncorporacion; }

        }


        public void Validar()
        {
            if (string.IsNullOrEmpty(_nombre)) throw new Exception("El nombre no puede ser vacio");
            if (string.IsNullOrEmpty(_apellido)) throw new Exception("El apellido no puede ser vacio");
            if (string.IsNullOrEmpty(_password) || _password.Length < 8) throw new Exception("La contraseña debe tener al menos 8 caracteres");
            if (_equipo == null) throw new Exception("El equipo no puede ser nulo");
            if (_fechaIncorporacion.CompareTo(DateTime.Now) == 1) throw new Exception("La fecha de ingreso no puede ser en el futuro");
        }

        public override string ToString()
        {
            return $"Nombre: {_nombre} {_apellido} - Email: {_email} - Equipo: {_equipo.Nombre}";
        }

        public override bool Equals(object? obj)
        {
            return obj is Usuario u && _email == u._email;
        }


        /// <summary>
        /// Genera mail base para un usuario instanciado para ser validado en sistema
        /// </summary>
        /// <returns>email</returns>
        public string GenerarSubDominioPrimario()
        {
            string nombre = _nombre;
            string apellido = _apellido;
            if (nombre.Length > 3)
            {
                nombre = nombre.Substring(0, 3);
            }
            if (apellido.Length > 3)
            {
                apellido = apellido.Substring(0, 3);
            }
            string emailPrimario = $"{nombre}{apellido}".ToLower();
            Email = emailPrimario;
            return emailPrimario;
        }


        /// <summary>
        /// valida que un usuario pertenezca a un equipo
        /// </summary>
        /// <param name="e">Equipo</param>
        /// <returns>true si el usuario pertenece al equipo</returns>
        public bool EsDeEquipo(Equipo e)
        {
            return e.Equals(_equipo);
        }

        public bool Loguea(string email, string pass)
        {
            return _email.Equals(email) && _password.Equals(pass);
        }

        public int CompareTo(Usuario? other)
        {
            return _email.CompareTo(other._email);
        }
    }
}
