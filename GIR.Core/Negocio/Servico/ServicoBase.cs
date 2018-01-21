using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using GIR.Core.Dados;
using GIR.Core.Dados.Entidade;
using GIR.Core.Dados.Infraestrutura;
using GIR.Core.Infraestrutura.Extensoes;
using GIR.Core.Negocio.Enum;
using GIR.Core.Negocio.Erro;

namespace GIR.Core.Negocio.Servico
{
    public abstract class ServicoBase
    {
        protected List<string> ListaDestinatarios;

        protected ServicoBase()
        {
            ListaDestinatarios = new List<string>();

        #if DEBUG
            var emails = ConfigurationManager.AppSettings["EmailTeste"];

            if (string.IsNullOrEmpty(emails)) return;

            var lista = emails.Split(',').ToList();
            ListaDestinatarios.AddRange(lista);

        #endif
        }

        protected void GravarLog(GIRContexto contexto, string loginUsuario, OperacaoLog operacaoLog, string descricao = null)
        {
            var repositorio = new RepositorioAtualizavel<Log>(contexto);
            try
            {
                var log = new Log()
                {
                    LoginUsuario = loginUsuario,
                    Id = (int)operacaoLog,
                    DescricaoUsuario = string.IsNullOrEmpty(descricao) ? operacaoLog.GetDescription() : descricao,
                    DataRegistroAcao = DateTime.Now
                };
                repositorio.Adicionar(log);
            }
            catch (NegocioException e)
            {
                var mensagem = "Erro ao criar Log para o usuário '" + loginUsuario + "'";
                throw new NegocioException(mensagem, e);
            }
        }
    }
}
