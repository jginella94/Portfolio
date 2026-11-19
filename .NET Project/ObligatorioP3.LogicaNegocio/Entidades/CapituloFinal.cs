using ObligatorioP3.LogicaNegocio.Vo;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioP3.LogicaNegocio.Entidades
{
    public class CapituloFinal : Capitulo
    {
        public int VecesAlcanzado { get; set; }

        protected CapituloFinal() { }

        public CapituloFinal(VoTitulo titulo, VoTexto texto) : base(titulo, texto) { }

        public void RegistrarLlegada()
        {
            VecesAlcanzado++;
        }
    }
}
