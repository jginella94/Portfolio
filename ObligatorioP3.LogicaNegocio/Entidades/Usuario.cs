using ObligatorioP3.LogicaNegocio.Vo;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioP3.LogicaNegocio.Entidades
{
    public abstract class Usuario
    {
        public int Id {  get; set; }
        public VoNombreCompleto NombreCompleto { get; private set; }
        public VoNombreUsuario NombreUsuario { get; private set; }
        public VoPass Password { get; private set; }
        public VoEmail Email { get; private set; }

        protected Usuario() { }
        public Usuario(VoNombreCompleto nombreCompleto, VoNombreUsuario nombreUsuario, VoPass password, VoEmail email)
        {
            NombreCompleto = nombreCompleto;
            NombreUsuario = nombreUsuario;
            Password = password;
            Email = email;
        }
    }
}
