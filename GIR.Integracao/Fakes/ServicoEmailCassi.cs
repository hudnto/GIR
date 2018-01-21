using GIR.Integracao.Fakes.Interace;
using GIR.Integracao.Model;
using GIR.Integracao.ServicoComunicacao;

namespace GIR.Integracao.Fakes
{
    public class ServicoEmailCassi : IServicoEmail
    {
        private readonly ServicoEmailPortClient _servico;

        public ServicoEmailCassi()
        {
            _servico = new ServicoEmailPortClient();
        }

        public override void EnviarEmail(EstruturaEmail estruturaEmail)
        {
            estruturaEmail.TipoConteudo = "text/html";
            estruturaEmail.IdentificadorSistema = "GIR";
            estruturaEmail.Remetente = "comprovantederendimentos@cassi.com.br";

            EnviarEmailRequest email = new EnviarEmailRequest
            {
                assunto = estruturaEmail.Assunto,
                conteudo = estruturaEmail.Corpo,
                de = estruturaEmail.Remetente,
                comCopiaPara = estruturaEmail.ComCopiaPara,
                copiaOcultaPara = estruturaEmail.ComCopiaOcultaPara,
                enderecoResposta = estruturaEmail.EnderecoResposta,
                identificadorSistema = estruturaEmail.IdentificadorSistema,
                para = estruturaEmail.Destinatarios.ToArray(),
                responderPara = estruturaEmail.ResponderPara,
                tipoConteudo = estruturaEmail.TipoConteudo
            };

            _servico.EnviarEmail(email);
        }
    }
}
