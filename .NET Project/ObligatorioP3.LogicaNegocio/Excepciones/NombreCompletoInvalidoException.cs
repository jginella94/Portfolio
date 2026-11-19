using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioP3.LogicaNegocio.Excepciones
{
    public class NombreCompletoInvalidoException : LogicaNegocioException
    {
        public NombreCompletoInvalidoException() { }

        public NombreCompletoInvalidoException(string msg) : base(msg) { }
    }
}
