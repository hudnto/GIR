using System;
using GIR.Core.Dados;
using GIR.Core.Dados.Entidade;
using GIR.Core.Dados.Infraestrutura;
using GIR.Core.Infraestrutura.Extensoes;
using GIR.Core.Negocio.Email.Fabricas;
using GIR.Core.Negocio.Enum;
using GIR.Core.Negocio.Erro;
using GIR.Integracao.Model;

namespace GIR.Core.Negocio.Email.Modelos
{
    public class Email
    {
        protected EstruturaEmail EstruturaEmail;

        public EstruturaEmail ConstruirEmailNotificacao(string assunto, string para, string nomeDestinatario, string anoExercicio, string anoCalendario, string enderecoArquivo)
        {
            EstruturaEmail estrutura = null;

            //estrutura.Assunto = assunto;
            //ConsultafornecedorEstruturaEmail.Corpo = FabricaEmail.Notificacao(para,null,null);

            return EstruturaEmail;
        }

        protected void GravarLog(GIRContexto contexto, string loginUsuario, OperacaoLog operacaoLog, string descricao = null)
        {
            var repositorio = new RepositorioAtualizavel<Log>(contexto);
            try
            {
                var log = new Log()
                {
                    LoginUsuario = loginUsuario,
                    //CodigoOperacaoRealizada = (int)operacaoLog,
                    DescricaoUsuario = String.IsNullOrEmpty(descricao) ? operacaoLog.GetDescription() : descricao,
                    DataRegistroAcao = DateTime.Now
                };
                repositorio.Adicionar(log);
            }
            catch (NegocioException e)
            {
                string mensagem = "Erro ao criar Log para o usuário '" + loginUsuario + "'";
                throw new NegocioException(mensagem, e);
            }
        }
    }
}
