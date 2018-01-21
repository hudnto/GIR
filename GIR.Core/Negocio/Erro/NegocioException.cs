using System;

namespace GIR.Core.Negocio.Erro
{
    public class NegocioException : Exception
    {
        public NegocioException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
        public NegocioException(string message)
            : base(message)
        {
        }
    }
}
