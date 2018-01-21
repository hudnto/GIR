using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace GIR.Core.Infraestrutura.Helpers
{
    public static class Helper
    {
        public static string NomedoCampoDaClasse<TClass>(Expression<Func<TClass, object>> exp)
        {
            MemberExpression body = exp.Body as MemberExpression;

            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)exp.Body;
                body = ubody.Operand as MemberExpression;
            }

            return body.Member.Name;
        }

        public static string RemoveContraBarra(this string valor)
        {
            if (valor == null)
                return String.Empty;

            // Ex: \\server\
            if ((valor[0] == '\\' && valor[1] == '\\') && (valor[valor.Length - 1] == '\\'))
            {
                var resultado = valor.Remove(0, 2);
                resultado = resultado.Remove(resultado.Length - 1);
                return resultado;
            }

            // Ex: \\server
            if ((valor[0] == '\\' && valor[1] == '\\'))
                return valor.Remove(0, 2);

            // Ex: \pasta\
            if (valor[0] == '\\' && valor[1] != '\\' && (valor[valor.Length - 1] == '\\'))
            {
                var resultado = valor.Remove(0, 1);
                resultado = resultado.Remove(resultado.Length - 1);
                return resultado;
            }

            // Ex: \pasta
            if (valor[0] == '\\' && valor[1] != '\\')
                return valor.Remove(0, 1);

            // Ex: server\ pasta\
            if (valor[valor.Length - 1] == '\\')
                return valor.Remove(valor.Length - 1);

            return valor;
        }
    }
}
