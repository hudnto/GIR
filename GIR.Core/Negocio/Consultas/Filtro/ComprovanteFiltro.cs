using System;
using GIR.Core.Negocio.Consultas.Interface;

namespace GIR.Core.Negocio.Consultas.Filtro
{
    public class ComprovanteFiltro : IFilter
    {
        public DateTime Data { get; set; }
        public string AreaRh { get; set; }
        public string CpfCnpj { get; set; }
        public string Email { get; set; }
        public string Uf { get; set; }
        public string NomeArquivo { get; set; }
    }
}
