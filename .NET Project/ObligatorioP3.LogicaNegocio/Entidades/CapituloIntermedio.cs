using ObligatorioP3.LogicaNegocio.Excepciones;
using ObligatorioP3.LogicaNegocio.Vo;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioP3.LogicaNegocio.Entidades
{
    public class CapituloIntermedio: Capitulo
    {
        private readonly List<Opcion> _opciones = new();
        public IReadOnlyCollection<Opcion> Opciones => _opciones.AsReadOnly();

        protected CapituloIntermedio() { }

        public CapituloIntermedio(VoTitulo titulo, VoTexto texto) : base(titulo, texto) { }

        public void AgregarOpcion(VoTextoOpcion texto, Capitulo destino)
        {
            if (destino is null)
                throw new OpcionInvalidaException("La opción debe tener un capítulo destino.");

            _opciones.Add(new Opcion(texto, destino));
        }

        public void ValidarParaPublicar()
        {
            if (_opciones.Count == 0)
                throw new CapituloInvalidoException("Un capítulo intermedio debe tener al menos una opción para publicar.");
        }
    }
}
