using System;
using GIR.Core.Negocio.Enum;

namespace GIR.Core.Negocio.DTO
{
    public class PesquisarContribuinteDTO
    {
        public String CpfCnpj { get; set; }
        public bool FoiEncontrado { get; set; }
        public TipoContribuinte TipoContribuinte { get; set; }
    }
}
