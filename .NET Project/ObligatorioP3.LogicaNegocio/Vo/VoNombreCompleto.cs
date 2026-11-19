using ObligatorioP3.LogicaNegocio.Excepciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ObligatorioP3.LogicaNegocio.Vo
{
    public record VoNombreCompleto
    {
        public string Value { get; private set; }

        public VoNombreCompleto(string value)
        {
            Value = value;
            Validar();
        }

        private void Validar()
        {
            if (string.IsNullOrEmpty(Value))
            {
                throw new NombreCompletoInvalidoException("El nombre completo no puede ser vacio.");
            }
        }
    }
}
