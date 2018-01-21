using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace GIR.Core.Negocio.Enum
{
    public enum TipoSituacao
    {
        [Description("Processado")]
        Processado,
        [Description("Enviado p/ Web")]
        EnviadoWeb,
        [Description("Erro")]
        Erro
    }
}
