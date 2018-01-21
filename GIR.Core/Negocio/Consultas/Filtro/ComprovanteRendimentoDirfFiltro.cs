using GIR.Core.Negocio.Consultas.Interface;

namespace GIR.Core.Negocio.Consultas.Filtro
{
    public class ComprovanteRendimentoDirfFiltro : IFilter
    {
        public int CodigoProcessamento { get; set; }
        public int UnidadeOrganzacionalId { get; set; }
        public string UnidadeOrganzacional { get; set; }
        public short UfId { get; set; }
        public string CpfCnpj { get; set; }
    }
}
