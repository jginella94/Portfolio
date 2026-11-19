namespace ObligatorioP3.LogicaAplicacion.DTOs.Capitulos
{
    public class DtoCapitulosParaAlta
    {
        public int HistoriaId { get; set; }
        public string TituloHistoria { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public List<DtoCapituloAntecedente> CapitulosIntermedios { get; set; } = new();
    }

    public class DtoCapituloAntecedente
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
    }
}
