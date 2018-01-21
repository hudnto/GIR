using GIR.Integracao.Fakes.Interace;
using GIR.Integracao.Model;
using GIR.Integracao.ServicoComunicacao;

namespace GIR.Integracao.Fakes
{
    public class ServicoComunicacao : IServicoComunicacao
    {
        private readonly ServicoEmailPortClient _servico;

        public ServicoComunicacao()
        {
            _servico = new ServicoEmailPortClient();
        }

        public void EnviarEmail(EstruturaEmail estruturaEmail)
        {
            EnviarEmailRequest email = new EnviarEmailRequest()
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
