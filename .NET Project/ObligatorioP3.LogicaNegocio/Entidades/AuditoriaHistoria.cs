using ObligatorioP3.LogicaNegocio.Excepciones;

namespace ObligatorioP3.LogicaNegocio.Entidades
{
    public class AuditoriaHistoria : IValidable
    {
        public int Id { get; private set; }
        public Historia? Historia { get; private set; }
        public int HistoriaIdHistorico { get; private set; }
        public Administrador Administrador { get; private set; } = null!;
        public string Accion { get; private set; } = string.Empty;
        public DateTime Fecha { get; private set; }

        private AuditoriaHistoria() { }

        public AuditoriaHistoria(Historia historia, Administrador administrador, string accion)
        {
            Historia = historia;
            HistoriaIdHistorico = historia?.Id ?? 0;
            Administrador = administrador;
            Accion = accion;
            Fecha = DateTime.UtcNow;
            Validar();
        }

        public void Validar()
        {
            if (HistoriaIdHistorico <= 0 || Administrador is null || string.IsNullOrWhiteSpace(Accion))
                throw new AuditoriaInvalidaException("La auditoría requiere historia, administrador y acción.");
        }
    }
}
