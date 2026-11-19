using Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public abstract class Pago : IValidable, IComparable<Pago>
    {
        private int _id;
        private static int s_id = 1;
        private MetodoPago _metodoPago;
        private TipoGasto _tipoGasto;
        private Usuario _usuario;
        private string _descripcion;
        private double _monto;

        public Pago(MetodoPago metodoPago, TipoGasto tipoGasto, Usuario usuario, string descripcion, double monto)
        {
            _id = s_id++;
            _metodoPago = metodoPago;
            _tipoGasto = tipoGasto;
            _usuario = usuario;
            _descripcion = descripcion;
            _monto = monto;
        }

        public int Id
        {
            get { return _id; }
        }

        public TipoGasto TipoGasto
        {
            get { return _tipoGasto; }
        }

        public string Descripcion
        {
            get { return _descripcion; }
        }

        public double Monto
        {
            get { return _monto; }
        }

        public Usuario Usuario
        {
            get { return _usuario; }
        }

        public MetodoPago MetodoPago
        {
            get { return _metodoPago; }
        }

        public virtual void Validar()
        {
            if (_tipoGasto == null) throw new Exception("El tipo de gasto no puede ser nulo");
            if (_usuario == null) throw new Exception("El usuario no puede ser nulo");
            if (string.IsNullOrEmpty(_descripcion)) throw new Exception("La descripción no puede ser vacia");
            if (_monto <= 0) throw new Exception("El monto debe ser positivo");
        }

        public abstract double CalcularGasto();


        public abstract bool EsMesCorriente();

        public abstract bool CorrespondeMesAnio(int m, int a);


        public abstract DateTime FechaAMostrar();

        public int CompareTo(Pago? other)
        {
            return CalcularGasto().CompareTo(other.CalcularGasto()) * -1;
        }
    }
}
