using Dominio.Interfaces;

namespace Dominio
{
    public class Equipo: IValidable
    {
        private int _id;
        private static int s_id = 1;
        private string _nombre;

        public Equipo(string nombre)
        {
            _id = s_id++;
            _nombre = nombre;
        }

        public string Nombre
        {
            get { return _nombre; }
        }

        public void Validar()
        {
            if (string.IsNullOrEmpty(_nombre)) throw new Exception("El nombre no puede ser vacio");
        }

        public override bool Equals(object? obj)
        {
            return obj is Equipo e && _nombre == e._nombre;
        }

    }

}
