using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioP3.LogicaNegocio.Excepciones
{
    public class EmailInvalidoException : LogicaNegocioException
    {
        public EmailInvalidoException() { }

        public EmailInvalidoException(string msg) : base(msg) { }
    }
}
