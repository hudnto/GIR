using GIR.Core.Negocio.Consultas.Filtro;

namespace GIR.Intranet.Models
{
    public class ConsultaComprovanteRendimentoVM
    {
        public int Codigo { get; set; }
        public string UnidadeOrganizacioanal { get; set; }
        public string Uf { get; set; }
        public string CpfCnpj { get; set; }

        public static ComprovanteRendimentoDirfFiltro Converter(ConsultaComprovanteRendimentoVM model)
        {
            var vm = new ComprovanteRendimentoDirfFiltro
            {
                CodigoProcessamento = model.Codigo,
                CpfCnpj = model.CpfCnpj,
                UnidadeOrganzacional= model.UnidadeOrganizacioanal
            };

            return vm;
        }
    }
}