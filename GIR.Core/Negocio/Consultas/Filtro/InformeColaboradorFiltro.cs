using GIR.Core.Negocio.Consultas.Interface;

namespace GIR.Core.Negocio.Consultas.Filtro
{
    /// <summary>
    /// Parâmetros para filtrar a consulta em informe de rendimentos do colaborador Cassi
    /// </summary>
    public class InformeColaboradorFiltro : IFilter
    {
        public int AnoExercicio { get; set; }
        public string CPFCNPJ { get; set; }
    }
}
