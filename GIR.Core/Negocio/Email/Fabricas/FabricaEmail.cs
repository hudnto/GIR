using GIR.Core.Negocio.DTO;
using GIR.Core.Negocio.Email.Modelos;
using GIR.Core.Negocio.Enum;
using GIR.Integracao.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace GIR.Core.Negocio.Email.Fabricas
{
    public class FabricaEmail
    {
        public static EstruturaEmail Notificacao(List<string> para, string nome, string link, TipoContribuinte tipoContribuinte, ProcessamentoComprovanteDTO arquivo)
        {
            var estrutura = new EstruturaEmail(para);

            var modelo = new StringBuilder(BuscarCorpoPara(tipoContribuinte));
            modelo.Replace("{DATA_COMPLETA}", DateTime.Now.ToString(@"dd \de MMMM \de yyyy"));
            modelo.Replace("{NOME}", nome);
            modelo.Replace("{ANO_EXERCICIO}", arquivo.AnoExercicio.ToString());
            modelo.Replace("{ANO_CALENDARIO}", arquivo.AnoCalendario.ToString());
            modelo.Replace("{LINKCOMPROVANTE}", link);

            estrutura.Assunto = "Comprovante do IR " + arquivo.AnoExercicio;
            estrutura.Corpo = modelo.ToString();

            return estrutura;
        }

        private static string BuscarCorpoPara(TipoContribuinte tipoContribuinte)
        {
            switch (tipoContribuinte)
            {
                case TipoContribuinte.FuncionarioCassi:
                    return ModeloEmail.NotificacaoFuncionarioCassi;
                case TipoContribuinte.PrestadorFornecedor:
                    return ModeloEmail.NotificacaoPrestadorFornecedor;
                case TipoContribuinte.Todos:
                    return ModeloEmail.NotificacaoFuncionarioCassi;
                default:
                    return ModeloEmail.NotificacaoFuncionarioCassi;
            }
        }
    }
}
