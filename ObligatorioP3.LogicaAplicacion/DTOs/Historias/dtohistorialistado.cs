namespace ObligatorioP3.LogicaAplicacion.DTOs.Historias
{
    public record DtoHistoriaListado(
        int Id,
        string Titulo,
        string Sinopsis,
        string Estado,
        List<int> Categorias,
        int IdCapituloInicial
       
    );
}