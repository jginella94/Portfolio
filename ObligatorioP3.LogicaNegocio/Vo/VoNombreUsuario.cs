using ObligatorioP3.LogicaNegocio.Excepciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ObligatorioP3.LogicaNegocio.Vo
{
    public record VoNombreUsuario
    {
        public string Value { get; private set; }

        public VoNombreUsuario(string value)
        {
            Value = value;
            Validar();
        }

        private void Validar()
        {
            if (string.IsNullOrEmpty(Value))
            {
                throw new NombreUsuarioInvalidoException("El Usuario no puede ser vacio.");
            }
        }
    }
}
