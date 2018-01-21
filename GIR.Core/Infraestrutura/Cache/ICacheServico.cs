using System;

namespace GIR.Core.Infraestrutura.Cache
{
    interface ICacheServico
    {
        T ObterOuAtribuir<T>(string chave, Func<T> retorno) where T : class;
    }
}
