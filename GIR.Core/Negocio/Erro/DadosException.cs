using System;

namespace GIR.Core.Negocio.Erro
{
    public class DadosException : Exception
    {
        public DadosException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
        public DadosException(string message)
            : base(message)
        {
        }
    }
}
