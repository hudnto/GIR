using System;
using GIR.Core.Dados;
using GIR.Core.Dados.Entidade;
using GIR.Core.Dados.Infraestrutura;
using GIR.Core.Infraestrutura.Extensoes;
using GIR.Core.Negocio.Enum;
using GIR.Core.Negocio.Erro;

namespace GIR.Core.Negocio.Servico
{
    public class LogServico
    {
        public LogServico()
        {
            
        }

        protected void CriarLogEntidade(Log entidade, OperacaoGIR operacao, string descricaoOperacao = null)
        {
            //var loginUsuario = entidade.LogOperacao.LoginUsuario;
            //entidade.LogOperacao = new LogOperacao();
            //CriarLog(entidade, operacao, loginUsuario, descricaoOperacao);
        }

        protected void CriarLogProjecao(Log entidade, string loginUsuario, OperacaoGIR operacao, string descricaoOperacao = null)
        {
            //entidade.LogOperacao = new LogOperacao();
            //entidade.LogOperacao.LoginUsuario = loginUsuario;

            //CriarLog(entidade, operacao, entidade.LogOperacao.LoginUsuario, descricaoOperacao);
        }

        //protected void AdicionarLogGeral(GIRContexto contexto, string loginUsuario, TipoLogEntidade tipoEntidade, int entidadeId, OperacaoSMS operacao, string descricaoOperacao = null)
        //{
        //    var repositorio = new RepositorioAtualizavel<Log>(contexto);
        //    var logOperacao = new LogOperacao();
        //    try
        //    {
        //        logOperacao.TipoEntidade = (int)tipoEntidade;
        //        logOperacao.EntidadeId = entidadeId;
        //        logOperacao.LoginUsuario = loginUsuario;
        //        logOperacao.OperacaoRealizada = (int)operacao;
        //        logOperacao.DescricaoOperacao = descricaoOperacao ?? operacao.GetDescription();
        //        logOperacao.DataOperacao = DateTime.Now;
        //        logOperacao.CodigoUnidade = _servicoColaborador.ObterUnidadeColaborador(loginUsuario);
        //        repositorio.Adicionar(logOperacao);
        //    }
        //    catch (Exception negocio)
        //    {
        //        string mensagem = "Erro ao criar Log Geral para o usuário '" + loginUsuario + "'";
        //        throw new NegocioException(mensagem, negocio);
        //    }
        //}

        private void CriarLog(Log entidade, OperacaoGIR operacao, string loginUsuario, string descricaoOperacao = null)
        {
            //try
            //{
            //    entidade.LogOperacao.TipoEntidade = (int)entidade.TipoEntidade;
            //    entidade.LogOperacao.EntidadeId = entidade.EntidadeId;
            //    entidade.LogOperacao.LoginUsuario = loginUsuario;
            //    entidade.LogOperacao.OperacaoRealizada = (int)operacao;
            //    entidade.LogOperacao.DescricaoOperacao = descricaoOperacao ?? operacao.GetDescription();
            //    entidade.LogOperacao.DataOperacao = DateTime.Now;
            //    entidade.LogOperacao.CodigoUnidade = _servicoColaborador.ObterUnidadeColaborador(loginUsuario);
            //}
            //catch (Exception negocio)
            //{
            //    string mensagem = "Erro ao criar Log para o usuário '" + loginUsuario + "'";
            //    throw new NegocioException(mensagem, negocio);
            //}
        }
    }
}
