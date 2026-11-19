using ObligatorioP3.LogicaNegocio.Excepciones;
using ObligatorioP3.LogicaNegocio.Vo;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioP3.LogicaNegocio.Entidades
{
    public class Opcion
    {
        public int Id { get; private set; }
        public VoTextoOpcion Texto { get; private set; } = null!;
        public Capitulo Destino { get; private set; } = null!;

        protected Opcion() { }

        public Opcion(VoTextoOpcion texto, Capitulo destino)
        {
            Texto = texto;
            Destino = destino;
        }
    }
}
