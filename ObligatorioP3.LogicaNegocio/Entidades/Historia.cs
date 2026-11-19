using ObligatorioP3.LogicaNegocio.Excepciones;
using ObligatorioP3.LogicaNegocio.Vo;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioP3.LogicaNegocio.Entidades
{
    public class Historia: IValidable
    {
        private readonly List<Categoria> _categorias = new();
        private readonly List<Capitulo> _capitulos = new();

        public int Id { get; private set; }
        public VoTitulo Titulo { get; private set; } = null!;
        public VoSinopsis Sinopsis { get; private set; } = null!;
        public EstadoHistoria Estado { get; private set; } = EstadoHistoria.BORRADOR;
        public CapituloIntermedio? CapituloInicial { get; private set; } = null!;
        public IReadOnlyCollection<Categoria> Categorias => _categorias.AsReadOnly();
        public IReadOnlyCollection<Capitulo> Capitulos => _capitulos.AsReadOnly();

        protected Historia() { }

        public Historia(VoTitulo titulo, VoSinopsis sinopsis, IEnumerable<Categoria> categorias, CapituloIntermedio? capituloInicial)
        {
            Titulo = titulo;
            Sinopsis = sinopsis;
            if (categorias is not null)
                _categorias.AddRange(categorias.Distinct());
            CapituloInicial = capituloInicial;
            if (capituloInicial is not null)
                _capitulos.Add(capituloInicial);
            Validar();
        }

        public void Modificar(VoTitulo titulo, VoSinopsis sinopsis, IEnumerable<Categoria> categorias)
        {
            Titulo = titulo;
            Sinopsis = sinopsis;
            if (categorias is null)
                throw new HistoriaInvalidaException("Debe indicar las categorías de la historia.");
            var nuevasCategorias = categorias.Distinct().ToList();
            if (nuevasCategorias.Count == 0 || nuevasCategorias.Any(c => c is null))
                throw new HistoriaInvalidaException("La historia debe tener al menos una categoría válida.");
            _categorias.Clear();
            _categorias.AddRange(nuevasCategorias);
            Validar();
        }

        public void ModificarCapituloInicial(VoTitulo titulo, VoTexto texto)
        {
            if (_capitulos.Count != 1)
                throw new HistoriaInvalidaException("No se puede modificar el capítulo inicial después de agregar otros capítulos.");
            CapituloInicial.ModificarContenido(titulo, texto);
        }

        public void AgregarCapitulo(CapituloIntermedio antecedente, Capitulo nuevoCapitulo, VoTextoOpcion textoOpcion)
        {
            if (Estado == EstadoHistoria.PUBLICADA)
                throw new HistoriaInvalidaException("No se pueden agregar capítulos a una historia publicada.");
            if (antecedente is null || !_capitulos.Contains(antecedente))
                throw new HistoriaInvalidaException("El capítulo antecedente no pertenece a esta historia.");
            if (nuevoCapitulo is null || _capitulos.Contains(nuevoCapitulo))
                throw new HistoriaInvalidaException("Debe proporcionar un capítulo nuevo.");
            antecedente.AgregarOpcion(textoOpcion, nuevoCapitulo);
            _capitulos.Add(nuevoCapitulo);
        }

        public void Publicar()
        {
            Validar();
            if (_capitulos.OfType<CapituloFinal>().Count() == 0)
                throw new HistoriaInvalidaException("La historia debe tener al menos un capítulo final.");
            foreach (Capitulo capitulo in _capitulos)
            {
                if (capitulo is CapituloIntermedio intermedio)
                {
                    intermedio.ValidarParaPublicar();
                    if (intermedio.Opciones.Any(o => !_capitulos.Contains(o.Destino)))
                        throw new HistoriaInvalidaException("Las opciones deben apuntar a capítulos de la misma historia.");
                }
            }
            Estado = EstadoHistoria.PUBLICADA;
        }

        public void Validar()
        {
            if (_categorias.Count == 0 || _categorias.Any(c => c is null))
                throw new HistoriaInvalidaException("La historia debe tener al menos una categoría válida.");
            if (CapituloInicial is null || !_capitulos.Contains(CapituloInicial))
                throw new HistoriaInvalidaException("La historia debe tener un capítulo inicial intermedio.");
        }
    }

}
