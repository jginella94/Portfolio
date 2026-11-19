using ObligatorioP3.LogicaNegocio.Vo;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioP3.LogicaNegocio.Entidades
{
    public abstract class Capitulo
    {
        public int Id { get; private set; }
        public VoTitulo Titulo { get; private set; }
        public VoTexto Texto { get; private set; }

        protected Capitulo() { }

        public Capitulo(VoTitulo titulo, VoTexto texto)
        {
            Titulo = titulo;
            Texto = texto;
        }

        //RELEVAR ESTO
        public void ModificarContenido(VoTitulo titulo, VoTexto texto)
        {
            Titulo = titulo;
            Texto = texto;
        }
    }
}
