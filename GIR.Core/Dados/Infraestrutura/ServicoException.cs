using System;

namespace GIR.Core.Dados.Infraestrutura
{
    public class ServicoException : Exception
    {
        public ServicoException(string mensagem)
            : base(mensagem)
        {
        }
        public ServicoException(string mensagem, Exception exception)
            : base(mensagem, exception)
        {

        }
    }
}
