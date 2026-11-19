using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioP3.LogicaNegocio.Excepciones
{
    public class NombreUsuarioInvalidoException : LogicaNegocioException
    {
        public NombreUsuarioInvalidoException() { }

        public NombreUsuarioInvalidoException(string msg) : base(msg) { }
    }
}
