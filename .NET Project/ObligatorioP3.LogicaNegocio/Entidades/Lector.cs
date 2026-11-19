using ObligatorioP3.LogicaNegocio.Vo;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioP3.LogicaNegocio.Entidades
{
    public class Lector: Usuario
    {
        protected Lector() { }
        public Lector(VoNombreCompleto nombreCompleto, VoNombreUsuario nombreUsuario, VoPass password, VoEmail email): base(nombreCompleto, nombreUsuario, password, email) { }
    }
}
