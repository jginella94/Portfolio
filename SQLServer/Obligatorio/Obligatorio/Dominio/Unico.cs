using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Unico : Pago
    {
        private DateTime _fechaRealizado;
        private int _numeroRecibo;

        public Unico(MetodoPago metodopago, TipoGasto tipogasto, Usuario usuario, string descripcion, double monto, DateTime fechaRealizado, int numeroRecibo):
            base(metodopago, tipogasto, usuario, descripcion, monto)
        {

            _fechaRealizado = fechaRealizado;
            _numeroRecibo = numeroRecibo;
        }


        /// <summary>
        /// Valida si un pago es de este mes
        /// </summary>
        /// <returns>true si lo es, false si no</returns>
        public override bool EsMesCorriente()
        {
           return _fechaRealizado.Month == DateTime.Now.Month && _fechaRealizado.Year == DateTime.Now.Year;
        }

        public override void Validar()
        {
            base.Validar();
            if (_numeroRecibo <= 0) throw new Exception("El recibo no puede ser menor a 0");
            if (_fechaRealizado.CompareTo(DateTime.Now) == 1) throw new Exception("La fecha no puede ser en el futuro");
        }

        /// <summary>
        /// Calcula monto total de un pago considerando recargos
        /// </summary>
        /// <returns>monto total del pago</returns>
        public override double CalcularGasto()
        {
            double descuento = 0.90;
            if (MetodoPago == MetodoPago.EFECTIVO) descuento = 0.80;

            return Monto * descuento;           
            
        }

        public override string ToString()
        {
            return "Unico";
        }

        public override DateTime FechaAMostrar()
        {
            return _fechaRealizado;
        }

        public override bool CorrespondeMesAnio(int m, int a)
        {
            return _fechaRealizado.Month == m && _fechaRealizado.Year == a;
        }
    }
}
