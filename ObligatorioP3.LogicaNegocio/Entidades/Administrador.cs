using ObligatorioP3.LogicaNegocio.Vo;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioP3.LogicaNegocio.Entidades
{
    public class Administrador: Usuario
    {
        protected Administrador() { }
        public Administrador(VoNombreCompleto nombreCompleto, VoNombreUsuario nombreUsuario, VoPass password, VoEmail email) : base(nombreCompleto, nombreUsuario, password, email) { }

    }
}
