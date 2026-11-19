using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Recurrente : Pago
    {
        private DateTime _fechaInicio;
        private DateTime? _fechaFin;

        public Recurrente(MetodoPago metodoPago, TipoGasto tipoGasto, Usuario usuario, string descripcion, double monto, DateTime fechaInicio, DateTime? fechaFin) :
            base(metodoPago, tipoGasto, usuario, descripcion, monto)
        {
            _fechaInicio = fechaInicio;
            _fechaFin = fechaFin;
        }

        public override void Validar()
        {
            base.Validar();
            if (_fechaInicio.CompareTo(DateTime.Now) == 1) throw new Exception("La fecha no puede ser en el futuro");
            if (_fechaFin != null && _fechaFin < _fechaInicio) throw new Exception("la fecha de fin debe ser mayor a la de inicio");
        }


        /// <summary>
        /// Calcula gasto total de las cuotas faltantes
        /// </summary>
        /// <returns>devuelve el monto de las cuotas si hay fechaFin, si no hay devulve el monto hasta el mes corriente</returns>
        public override double CalcularGasto()
        {
            int cuotas = 1;

            double recargo = 1.03;
            if (_fechaFin != null)
            {
                cuotas = ContarMeses();
                if (cuotas >= 10)
                {
                    recargo = 1.10;
                }
                else if (cuotas >= 6 && cuotas <= 9)
                {
                    recargo = 1.05;
                }
            }
            return (Monto * recargo) / cuotas;
        }


        /// <summary>
        /// cuenta la cantidad de cuotas.
        /// </summary>
        /// <returns>devuelve el total de cuotas, si no fechaFin devuelve la cantidad de cuotas a la fecha de hoy </returns>
        private int ContarMeses()
        {
            DateTime fechaFinal = DateTime.Now;
            if (_fechaFin != null) fechaFinal = (DateTime)_fechaFin;

            return ((fechaFinal.Year - _fechaInicio.Year) * 12) + fechaFinal.Month - _fechaInicio.Month;
        }

        public override bool EsMesCorriente()
        {
            
            DateTime fechaActual = DateTime.Now;
            bool validFechaInicio = _fechaInicio.Month <= DateTime.Now.Month && _fechaInicio.Year <= DateTime.Now.Year;
            bool validFechaFin = true;
            if (_fechaFin != null)
            {
                DateTime fechaFinal = (DateTime)_fechaFin;
                validFechaFin = ((fechaFinal.Year - fechaActual.Year) * 12) + fechaFinal.Month - fechaActual.Month >=0;
                
            }

            return validFechaInicio && validFechaFin;
        }



       
        public override string ToString()
        {
            return "Recurrente";
        }

        public override DateTime FechaAMostrar()
        {
            return _fechaInicio;
        }

        public override bool CorrespondeMesAnio(int m, int a)
        {
            bool validFechaInicio = _fechaInicio.Month <= m && _fechaInicio.Year <= a;
            bool validFechaFin = true;
            if (_fechaFin != null)
            {
                DateTime fechaFinal = (DateTime)_fechaFin;
                validFechaFin = ((fechaFinal.Year - a) * 12) + fechaFinal.Month - m >= 0;

            }

            return validFechaInicio && validFechaFin;
        }

    
    }


}
