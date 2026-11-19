using ObligatorioP3.LogicaNegocio.Excepciones;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioP3.LogicaNegocio.Vo
{
    public record VoEmail
    {
        public string Value { get; private set; }

        public VoEmail(string value)
        {
            Value = value.Trim();
            Validar();
        }

        private void Validar()
        {
            if (string.IsNullOrEmpty(Value))
            {
                throw new EmailInvalidoException("El email está vacío.");
            }
            if (!Value.Contains("@"))
            {
                throw new EmailInvalidoException("El email no es válido.");
            }
        }
    }
}
