using Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class TipoGasto : IValidable
    {
        private string _nombre;
        private string _descripcion;

        public TipoGasto(string nombre, string descripcion)
        {
            _nombre = nombre;
            _descripcion = descripcion;
        }

        public string Nombre
        {
            get {  return _nombre; }
        } 

        public void Validar()
        {
            if (string.IsNullOrEmpty(_nombre)) throw new Exception("El nombre no puede ser vacio");
            if (string.IsNullOrEmpty(_descripcion)) throw new Exception("La descripcíon no puede ser vacia");
        }

        public override bool Equals(object? obj)
        {
            return obj is TipoGasto tg && _nombre == tg._nombre;
        }
    }
}
