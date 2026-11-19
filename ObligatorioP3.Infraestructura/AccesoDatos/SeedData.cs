using Microsoft.EntityFrameworkCore;
using EFCore = Microsoft.EntityFrameworkCore.EF;
using ObligatorioP3.Infraestructura.AccesoDatos.EF;
using ObligatorioP3.Infraestructura.EF.AccesoDatos;
using ObligatorioP3.LogicaNegocio.Entidades;
using ObligatorioP3.LogicaNegocio.Vo;

namespace ObligatorioP3.Infraestructura.AccesoDatos
{
    public class SeedData
    {
        private readonly ObligatorioP3Context _context;

        private static readonly string[] NombresCategorias =
        {
            "Terror", "Fantasía", "Ciencia Ficción", "Aventura", "Misterio",
            "Suspenso", "Drama", "Romance", "Historia", "Humor"
        };

        private static readonly string[] TitulosHistorias =
        {
            "El faro de las sombras", "El bosque de cristal", "La estación olvidada",
            "El mapa del corsario", "El reloj de la torre", "La ciudad bajo el lago",
            "La llave del museo", "La última biblioteca", "El jardín de invierno",
            "La isla sin nombre"
        };

        private static readonly string[] NombresLectores =
        {
            "Ana Pereira", "Bruno Silva", "Carla Méndez", "Diego Fernández",
            "Elena Rodríguez", "Felipe Castro", "Gabriela Núñez", "Hugo Martín",
            "Inés Cabrera", "Joaquín López", "Lucía Torres", "Mateo Acosta"
        };

        private static readonly (int Historia, int[] Lectores, string Finales)[] Recorridos =
        {
            (0, new[] { 0, 0, 0, 1, 1, 2, 3, 4, 5 }, "AAAABBBBC"),
            (1, new[] { 0, 0, 1, 2, 3, 4, 6 }, "AAAABBC"),
            (2, new[] { 0, 1, 1, 2, 3, 7 }, "AAABBC"),
            (3, new[] { 0, 1, 2, 3, 8 }, "AABBC"),
            (4, new[] { 0, 1, 2, 9 }, "AABC"),
            (5, new[] { 0, 1, 2 }, "AAB"),
            (6, new[] { 0, 1 }, "AB")
        };

        public SeedData(ObligatorioP3Context context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (_context.Historias.Any(h => h.Titulo.Value == TitulosHistorias[0]))
                return;

            if (_context.Historias.Any() || _context.Categorias.Any() ||
                _context.Usuarios.Any() || _context.Capitulos.Any() ||
                _context.Opciones.Any() || _context.Lecturas.Any() ||
                _context.AuditoriasHistoria.Any())
                throw new InvalidOperationException("La precarga requiere una base vacía o previamente precargada por este Seed.");

            using var transaccion = _context.Database.BeginTransaction();
            try
            {
                List<Categoria> categorias = CrearCategorias();
                (List<Administrador> administradores, List<Lector> lectores) = CrearUsuarios();
                List<Historia> historias = CrearHistorias(categorias, administradores);
                CrearCapitulosYOpciones(historias, administradores);
                PublicarHistorias(historias, administradores);
                _context.SaveChanges();

                _context.ChangeTracker.Clear();
                List<Historia> historiasPersistidas = CargarHistorias();
                ValidarHistorias(historiasPersistidas);
                CrearLecturas(historiasPersistidas);
                ValidarResultados();

                transaccion.Commit();
                _context.ChangeTracker.Clear();
            }
            catch
            {
                transaccion.Rollback();
                _context.ChangeTracker.Clear();
                throw;
            }
        }

        private List<Categoria> CrearCategorias()
        {
            List<Categoria> categorias = NombresCategorias
                .Select(nombre => new Categoria(new VoNombreCategoria(nombre)))
                .ToList();
            _context.Categorias.AddRange(categorias);
            _context.SaveChanges();
            return categorias;
        }

        private (List<Administrador>, List<Lector>) CrearUsuarios()
        {
            List<Administrador> administradores = new()
            {
                new Administrador(new VoNombreCompleto("Mariana Gómez"), new VoNombreUsuario("admin.mariana"), new VoPass("Prueba2026!"), new VoEmail("admin.mariana@aventura.test")),
                new Administrador(new VoNombreCompleto("Pablo Ramírez"), new VoNombreUsuario("admin.pablo"), new VoPass("Prueba2026!"), new VoEmail("admin.pablo@aventura.test"))
            };

            List<Lector> lectores = new();
            for (int i = 0; i < NombresLectores.Length; i++)
            {
                string usuario = $"lector{i + 1:00}";
                lectores.Add(new Lector(
                    new VoNombreCompleto(NombresLectores[i]),
                    new VoNombreUsuario(usuario),
                    new VoPass("Prueba2026!"),
                    new VoEmail($"{usuario}@aventura.test")));
            }

            _context.Administradores.AddRange(administradores);
            _context.Lectores.AddRange(lectores);
            _context.SaveChanges();
            return (administradores, lectores);
        }

        private List<Historia> CrearHistorias(List<Categoria> categorias, List<Administrador> administradores)
        {
            List<Historia> historias = new();
            for (int i = 0; i < TitulosHistorias.Length; i++)
            {
                CapituloIntermedio inicial = new(
                    new VoTitulo($"{TitulosHistorias[i]} - Comienzo"),
                    new VoTexto($"La aventura de {TitulosHistorias[i]} comienza con una decisión inesperada."));

                Historia historia = new(
                    new VoTitulo(TitulosHistorias[i]),
                    new VoSinopsis($"Una aventura interactiva sobre {TitulosHistorias[i].ToLowerInvariant()}, con diferentes decisiones y desenlaces."),
                    new[] { categorias[i], categorias[(i + 3) % categorias.Count] },
                    inicial);

                GuardarHistoriaConInicial(historia);
                historias.Add(historia);
                RegistrarAuditoria(historia, administradores[i % administradores.Count], "CREACION_HISTORIA");
            }
            _context.SaveChanges();
            return historias;
        }

        private void GuardarHistoriaConInicial(Historia historia)
        {
            CapituloIntermedio inicial = historia.CapituloInicial!;
            _context.Historias.Add(historia);
            _context.Entry(inicial).State = EntityState.Detached;
            _context.Entry(historia).Reference(h => h.CapituloInicial).CurrentValue = null;

            bool detectarCambios = _context.ChangeTracker.AutoDetectChangesEnabled;
            try
            {
                _context.ChangeTracker.AutoDetectChangesEnabled = false;
                _context.SaveChanges();
            }
            finally
            {
                _context.ChangeTracker.AutoDetectChangesEnabled = detectarCambios;
            }

            _context.Entry(historia).Reference(h => h.CapituloInicial).CurrentValue = inicial;
            _context.Entry(inicial).State = EntityState.Added;
            _context.ChangeTracker.DetectChanges();
            _context.Entry(inicial).Property("HistoriaId").CurrentValue = historia.Id;
            _context.SaveChanges();
        }

        private void CrearCapitulosYOpciones(List<Historia> historias, List<Administrador> administradores)
        {
            for (int i = 0; i < historias.Count; i++)
            {
                Historia historia = historias[i];
                CapituloIntermedio inicial = historia.CapituloInicial!;
                CapituloIntermedio intermedio = new(
                    new VoTitulo($"{TitulosHistorias[i]} - El cruce"),
                    new VoTexto("Hay dos caminos posibles y ambos parecen conducir a una revelación."));
                CapituloFinal finalA = new(
                    new VoTitulo($"{TitulosHistorias[i]} - Final A: Regreso"),
                    new VoTexto("La persona protagonista consigue regresar a casa."));
                CapituloFinal finalB = new(
                    new VoTitulo($"{TitulosHistorias[i]} - Final B: Descubrimiento"),
                    new VoTexto("El misterio se resuelve y aparece una nueva verdad."));
                CapituloFinal finalC = new(
                    new VoTitulo($"{TitulosHistorias[i]} - Final C: Sacrificio"),
                    new VoTexto("La decisión final salva a los demás, pero tiene un costo."));

                historia.AgregarCapitulo(inicial, intermedio, new VoTextoOpcion("Explorar el cruce"));
                _context.SaveChanges();
                historia.AgregarCapitulo(inicial, finalA, new VoTextoOpcion("Regresar a casa"));
                _context.SaveChanges();
                historia.AgregarCapitulo(intermedio, finalB, new VoTextoOpcion("Investigar el misterio"));
                _context.SaveChanges();
                historia.AgregarCapitulo(intermedio, finalC, new VoTextoOpcion("Proteger al grupo"));
                _context.SaveChanges();

                RegistrarAuditoria(historia, administradores[i % administradores.Count], "ALTA_CAPITULOS");
            }
            _context.SaveChanges();
        }

        private void PublicarHistorias(List<Historia> historias, List<Administrador> administradores)
        {
            for (int i = 0; i < 8; i++)
            {
                historias[i].Publicar();
                RegistrarAuditoria(historias[i], administradores[i % administradores.Count], "PUBLICACION_HISTORIA");
            }
        }

        private void RegistrarAuditoria(Historia historia, Administrador administrador, string accion)
        {
            _context.AuditoriasHistoria.Add(new AuditoriaHistoria(historia, administrador, accion));
        }

        private List<Historia> CargarHistorias()
        {
            RepositorioHistoria repositorio = new(_context);
            List<Historia> historias = new();
            foreach (string titulo in TitulosHistorias)
            {
                int id = _context.Historias
                    .Where(h => h.Titulo.Value == titulo)
                    .Select(h => h.Id)
                    .Single();
                historias.Add(repositorio.GetById(id));
            }
            return historias;
        }

        private void ValidarHistorias(List<Historia> historias)
        {
            foreach (Historia historia in historias)
            {
                int[] ids = historia.Capitulos.Select(c => c.Id).ToArray();
                if (ids.Length != 5 || ids.Distinct().Count() != 5)
                    throw new InvalidOperationException($"Capítulos incorrectos en {historia.Titulo.Value}.");

                foreach (CapituloIntermedio intermedio in historia.Capitulos.OfType<CapituloIntermedio>())
                {
                    if (intermedio.Opciones.Count == 0)
                        throw new InvalidOperationException($"Capítulo intermedio sin opciones: {intermedio.Id}.");

                    foreach (Opcion opcion in intermedio.Opciones)
                    {
                        if (opcion.Id <= 0 || !ids.Contains(opcion.Destino.Id) ||
                            !_context.Opciones.Any(o => o.Id == opcion.Id &&
                                EFCore.Property<int>(o, "DestinoId") == opcion.Destino.Id &&
                                EFCore.Property<int?>(o, "CapituloIntermedioId") == intermedio.Id))
                            throw new InvalidOperationException($"Opción o destino incorrecto en {historia.Titulo.Value}.");
                    }
                }

                int opciones = historia.Capitulos.OfType<CapituloIntermedio>().Sum(c => c.Opciones.Count);
                if (opciones != 4)
                    throw new InvalidOperationException($"Cantidad de opciones incorrecta en {historia.Titulo.Value}.");
            }
        }

        private void CrearLecturas(List<Historia> historias)
        {
            List<Lector> lectores = _context.Lectores
                .OrderBy(u => u.NombreUsuario.Value)
                .ToList();

            int indice = 0;
            foreach (var recorrido in Recorridos)
            {
                for (int i = 0; i < recorrido.Lectores.Length; i++)
                {
                    Lectura lectura = new(lectores[recorrido.Lectores[i]], historias[recorrido.Historia]);
                    _context.Lecturas.Add(lectura);
                    Avanzar(lectura, recorrido.Finales[i]);
                    AplicarFechaHistorica(lectura, indice++);
                    _context.SaveChanges();
                }
            }

            (int Historia, int Lector, bool Avanzar)[] pendientes =
            {
                (0, 10, false), (1, 11, true), (2, 10, true),
                (4, 11, false), (6, 10, false), (7, 11, true)
            };

            foreach (var pendiente in pendientes)
            {
                Lectura lectura = new(lectores[pendiente.Lector], historias[pendiente.Historia]);
                _context.Lecturas.Add(lectura);
                if (pendiente.Avanzar)
                    Avanzar(lectura, 'P');
                AplicarFechaHistorica(lectura, indice++);
                _context.SaveChanges();
            }
        }

        private void Avanzar(Lectura lectura, char resultado)
        {
            CapituloIntermedio inicial = (CapituloIntermedio)lectura.CapituloActual;
            Opcion opcionInicial = inicial.Opciones.Single(o =>
                resultado == 'A' ? o.Destino is CapituloFinal : o.Destino is CapituloIntermedio);
            lectura.ElegirOpcion(opcionInicial);

            if (resultado is 'B' or 'C')
            {
                CapituloIntermedio intermedio = (CapituloIntermedio)lectura.CapituloActual;
                string sufijo = resultado == 'B' ? "Final B:" : "Final C:";
                Opcion siguiente = intermedio.Opciones.Single(o => o.Destino.Titulo.Value.Contains(sufijo));
                lectura.ElegirOpcion(siguiente);
            }
        }

        private void AplicarFechaHistorica(Lectura lectura, int indice)
        {
            DateTime inicio = new(
                2024 + indice % 2,
                1 + (indice / 2 % 6) * 2,
                10 + indice % 12,
                10, 0, 0, DateTimeKind.Utc);
            _context.Entry(lectura).Property(l => l.FechaInicio).CurrentValue = inicio;

            if (lectura.Completada)
                _context.Entry(lectura).Property(l => l.FechaFinalizacion).CurrentValue = inicio.AddMinutes(5 + indice % 25);
        }

        private void ValidarResultados()
        {
            if (_context.Categorias.Count() != 10 || _context.Usuarios.Count() != 14 ||
                _context.Historias.Count() != 10 || _context.Capitulos.Count() != 50 ||
                _context.Opciones.Count() != 40 || _context.Lecturas.Count() != 42 ||
                _context.AuditoriasHistoria.Count() != 28)
                throw new InvalidOperationException("Las cantidades de precarga no coinciden con las previstas.");

            if (_context.Lecturas.Count(l => l.FechaFinalizacion != null) != 36 ||
                _context.Lecturas.Count(l => l.FechaFinalizacion == null) != 6 ||
                _context.Lecturas.Any(l => l.FechaFinalizacion != null && l.FechaFinalizacion <= l.FechaInicio))
                throw new InvalidOperationException("Las fechas o estados de lectura no son coherentes.");

            int[] rankingEsperado = { 9, 7, 6, 5, 4, 3, 2 };
            int[] ranking = _context.Lecturas
                .Where(l => l.FechaFinalizacion != null)
                .GroupBy(l => EFCore.Property<int>(l, "HistoriaId"))
                .Select(g => g.Count())
                .OrderByDescending(n => n)
                .ToArray();
            if (!ranking.SequenceEqual(rankingEsperado))
                throw new InvalidOperationException("RF09 no coincide con las lecturas previstas.");

            int[] lectoresEsperados = { 7, 7, 6, 4, 2, 1, 1, 1, 1, 1 };
            int[] rankingLectores = _context.Lecturas
                .Where(l => l.FechaFinalizacion != null)
                .GroupBy(l => EFCore.Property<int>(l, "UsuarioId"))
                .Select(g => g.Select(l => EFCore.Property<int>(l, "HistoriaId")).Distinct().Count())
                .OrderByDescending(n => n)
                .ToArray();
            if (!rankingLectores.SequenceEqual(lectoresEsperados))
                throw new InvalidOperationException("RF11 no coincide con las lecturas previstas.");

            int[][] frecuencias =
            {
                new[] { 4, 4, 1 }, new[] { 4, 2, 1 }, new[] { 3, 2, 1 },
                new[] { 2, 2, 1 }, new[] { 2, 1, 1 }, new[] { 2, 1, 0 },
                new[] { 1, 1, 0 }, new[] { 0, 0, 0 }
            };
            for (int i = 0; i < frecuencias.Length; i++)
            {
                string titulo = TitulosHistorias[i];
                int historiaId = _context.Historias.Where(h => h.Titulo.Value == titulo).Select(h => h.Id).Single();
                int[] finales = _context.CapitulosFinales
                    .Where(c => EFCore.Property<int?>(c, "HistoriaId") == historiaId)
                    .OrderBy(c => c.Titulo.Value)
                    .Select(c => c.VecesAlcanzado)
                    .ToArray();
                if (!finales.SequenceEqual(frecuencias[i]))
                    throw new InvalidOperationException($"RF10 no coincide para {titulo}.");

                int completadas = _context.Lecturas.Count(l =>
                    EFCore.Property<int>(l, "HistoriaId") == historiaId && l.FechaFinalizacion != null);
                if (completadas != finales.Sum())
                    throw new InvalidOperationException($"Los finales no coinciden con las lecturas de {titulo}.");
            }
        }
    }
}
