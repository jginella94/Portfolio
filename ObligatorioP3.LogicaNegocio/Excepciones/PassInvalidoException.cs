using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioP3.LogicaNegocio.Excepciones
{
    public class PassInvalidoException: LogicaNegocioException
    {
        public PassInvalidoException() { }

        public PassInvalidoException(string msg) : base(msg) { }
    }
}
