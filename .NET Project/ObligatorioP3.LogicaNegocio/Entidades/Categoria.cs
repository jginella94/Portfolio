using ObligatorioP3.LogicaNegocio.Vo;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioP3.LogicaNegocio.Entidades
{
    public class Categoria
    {
        public int Id { get; private set; }
        public VoNombreCategoria Nombre { get; private set; } = null!;

        protected Categoria() { }

        public Categoria(VoNombreCategoria nombre)
        {
            Nombre = nombre;
        }

        public void CambiarNombre(VoNombreCategoria nombre)
        {
            Nombre = nombre;
        }
    }
}
