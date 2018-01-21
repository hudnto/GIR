using System.Collections.Generic;

namespace GIR.Integracao.Model
{
    public class EstruturaEmail
    {
        public EstruturaEmail(List<string> destinatarios)
        {
            Destinatarios = destinatarios;
        }
        public string Assunto { get; set; }
        public List<string> Destinatarios { get; set; }
        public string Corpo { get; set; }
        public string Remetente { get; set; }
        public string ResponderPara { get; set; }
        public string ComCopiaOcultaPara { get; set; }
        public string ComCopiaPara { get; set; }
        public string EnderecoResposta { get; set; }
        public string IdentificadorSistema { get; set; }
        public string TipoConteudo { get; set; }
    }
}
