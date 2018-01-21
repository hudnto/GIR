using System.ComponentModel;

namespace GIR.Core.Negocio.Enum
{
    public enum TipoContribuinte
    {
        [Description("Funcionários Cassi")] FuncionarioCassi = 0,
        [Description("Prestador / Fornecedor")] PrestadorFornecedor = 1,
        [Description("Todos")] Todos = 2,
        Nenhum
    }
}
